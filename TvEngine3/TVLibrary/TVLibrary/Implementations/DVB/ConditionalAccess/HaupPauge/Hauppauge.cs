using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using DirectShowLib;
using DirectShowLib.BDA;
using System.Windows.Forms;
using TvLibrary.Channels;
using TvLibrary.Interfaces.Analyzer;

namespace TvLibrary.Implementations.DVB
{
  public class Hauppauge : IDiSEqCController
  {
    #region enums
    enum BdaModes
    {
      BDA_TUNER_NODE = 0,
      BDA_DEMODULATOR_NODE
    };
    enum BdaTunerExtension
    {
      KSPROPERTY_BDA_DISEQC = 0,
      KSPROPERTY_BDA_PILOT = 0x20,
      KSPROPERTY_BDA_ROLL_OFF = 0x21
    };
    enum DisEqcVersion
    {
      DISEQC_VER_1X = 1,
      DISEQC_VER_2X,
      ECHOSTAR_LEGACY,	// (not supported)
      DISEQC_VER_UNDEF = 0	// undefined (results in an error)
    };
    enum RxMode
    {
      RXMODE_INTERROGATION = 1, // Expecting multiple devices attached
      RXMODE_QUICKREPLY,      // Expecting 1 rx (rx is suspended after 1st rx received)
      RXMODE_NOREPLY,         // Expecting to receive no Rx message(s)
      RXMODE_DEFAULT = 0        // use current register setting
    };
    enum BurstModulationType
    {
      TONE_BURST_UNMODULATED = 0,
      TONE_BURST_MODULATED
    };
    enum RollOff
    {
      HCW_ROLL_OFF_NOT_SET = -1,
      HCW_ROLL_OFF_NOT_DEFINED = 0,
      HCW_ROLL_OFF_20 = 1,         // .20 Roll Off (DVB-S2 Only)
      HCW_ROLL_OFF_25,             // .25 Roll Off (DVB-S2 Only)
      HCW_ROLL_OFF_35,             // .35 Roll Off (DVB-S2 Only)
      HCW_ROLL_OFF_MAX
    };
    enum Pilot
    {
      HCW_PILOT_NOT_SET = -1,
      HCW_PILOT_NOT_DEFINED = 0,
      HCW_PILOT_OFF = 1,           // Pilot Off (DVB-S2 Only)
      HCW_PILOT_ON,                // Pilot On  (DVB-S2 Only)
      HCW_PILOT_MAX
    }
    #endregion

    #region constants
    const byte DISEQC_TX_BUFFER_SIZE = 150;	// 3 bytes per message * 50 messages
    const byte DISEQC_RX_BUFFER_SIZE = 8;		// reply fifo size, do not increase
    #endregion

    #region structs
    /*
    [StructLayout(LayoutKind.Sequential), ComVisible(true)]
    struct DISEQC_MESSAGE_PARAMS
    {
     byte       uc_diseqc_send_message[DISEQC_TX_BUFFER_SIZE+1];    //0-150
     byte       uc_diseqc_receive_message[DISEQC_RX_BUFFER_SIZE+1]; //151-159
     ulong      ul_diseqc_send_message_length;                      //160..163
     ulong      ul_diseqc_receive_message_length;                   //164..167
     ulong      ul_amplitude_attenuation;                           //168..171
     bool       b_tone_burst_modulated;                             //172
     DISEQC_VER diseqc_version;                                     //173
     RXMODE     receive_mode;                                       //174
     bool       b_last_message;                                     //175
    };
    */

    #endregion

    #region variables
    Guid BdaTunerExtentionProperties = new Guid(0xfaa8f3e5, 0x31d4, 0x4e41, 0x88, 0xef, 0x00, 0xa0, 0xc9, 0xf2, 0x1f, 0xc7);
    bool _isHauppauge = false;
    IntPtr _ptrDiseqc = IntPtr.Zero;
    DirectShowLib.IKsPropertySet _propertySet=null;
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Hauppauge"/> class.
    /// </summary>
    /// <param name="tunerFilter">The tuner filter.</param>
    /// <param name="analyzerFilter">The analyzer filter.</param>
    public Hauppauge(IBaseFilter tunerFilter, IBaseFilter analyzerFilter)
    {
      IPin pin = DsFindPin.ByDirection(tunerFilter, PinDirection.Input, 0);
      if (pin != null)
      {
        _propertySet = pin as DirectShowLib.IKsPropertySet;
        if (_propertySet != null)
        {
          KSPropertySupport supported;
          _propertySet.QuerySupported(BdaTunerExtentionProperties, (int)BdaTunerExtension.KSPROPERTY_BDA_DISEQC, out supported);
          if ((supported & KSPropertySupport.Set) != 0)
          {
            _isHauppauge = true;
            _ptrDiseqc = Marshal.AllocCoTaskMem(1024);
          }
        }
      }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is hauppauge.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is hauppauge; otherwise, <c>false</c>.
    /// </value>
    public bool IsHauppauge
    {
      get
      {
        return _isHauppauge;
      }
    }
    /// <summary>
    /// Sends the diseq command.
    /// </summary>
    /// <param name="channel">The channel.</param>
    public void SendDiseqCommand(DVBSChannel channel)
    {
      if (_isHauppauge == false) return;
      int position = 0;
      int option = 0;
      bool hiBand = false;

      switch (channel.BandType)
      {
        case BandType.Universal:
          if (channel.Frequency >= 11700000)
          {
            hiBand = true;
          }
          else
          {
            hiBand = false;
          }
          break;
      }

      switch (channel.DisEqc)
      {
        case DisEqcType.None:
        case DisEqcType.SimpleA://simple A
          position = 0;
          option = 0;
          break;
        case DisEqcType.SimpleB://simple B
          position = 0;
          option = 0;
          break;
        case DisEqcType.Level1AA://Level 1 A/A
          position = 0;
          option = 0;
          break;
        case DisEqcType.Level1BA://Level 1 B/A
          position = 1;
          option = 0;
          break;
        case DisEqcType.Level1AB://Level 1 A/B
          position = 0;
          option = 1;
          break;
        case DisEqcType.Level1BB://Level 1 B/B
          position = 1;
          option = 1;
          break;
      }
      bool vertical = (channel.Polarisation == Polarisation.LinearV);
      uint diseqc = 0xE01038F0;
      if (hiBand)                 // high band
        diseqc |= 0x00000001;
      else                        // low band
        diseqc &= 0xFFFFFFFE;

      if (vertical)             // vertikal
        diseqc &= 0xFFFFFFFD;
      else                        // horizontal
        diseqc |= 0x00000002;

      if (position != 0)             // Sat B
        diseqc |= 0x00000004;
      else                        // Sat A
        diseqc &= 0xFFFFFFFB;

      if (option != 0)               // option B
        diseqc |= 0x00000008;
      else                        // option A
        diseqc &= 0xFFFFFFF7;


      int len = 188;//sizeof(DISEQC_MESSAGE_PARAMS);

      Marshal.WriteByte(_ptrDiseqc, 0, (byte)((diseqc >> 24) & 0xff));
      Marshal.WriteByte(_ptrDiseqc, 1, (byte)((diseqc >> 16) & 0xff));
      Marshal.WriteByte(_ptrDiseqc, 2, (byte)((diseqc >> 8) & 0xff));
      Marshal.WriteByte(_ptrDiseqc, 3, (byte)(diseqc & 0xff));
      Marshal.WriteInt32(_ptrDiseqc, 160, (Int32)4);//send_message_length
      Marshal.WriteInt32(_ptrDiseqc, 164, (Int32)0);//receive_message_length
      Marshal.WriteInt32(_ptrDiseqc, 168, (Int32)3);//amplitude_attenuation
      Marshal.WriteByte(_ptrDiseqc, 172, 1);//tone_burst_modulated
      Marshal.WriteByte(_ptrDiseqc, 176, (int)DisEqcVersion.DISEQC_VER_1X);
      Marshal.WriteByte(_ptrDiseqc, 180, (int)RxMode.RXMODE_NOREPLY);
      Marshal.WriteByte(_ptrDiseqc, 184, 1);//last_message

      int hr=_propertySet.Set(BdaTunerExtentionProperties, (int)BdaTunerExtension.KSPROPERTY_BDA_DISEQC, _ptrDiseqc, len, _ptrDiseqc, len);
      Log.Log.Info("hauppauge: setdiseqc returned:{0:X}", hr);
    }

    #region IDiSEqCController Members

    /// <summary>
    /// Sends the DiSEqC command.
    /// </summary>
    /// <param name="diSEqC">The DiSEqC command.</param>
    /// <returns>true if succeeded, otherwise false</returns>
    public bool SendDiSEqCCommand(byte[] diSEqC)
    {
      int len = 188;//sizeof(DISEQC_MESSAGE_PARAMS);
      for (int i=0; i < diSEqC.Length;++i)
        Marshal.WriteByte(_ptrDiseqc, i, diSEqC[i]);
      Marshal.WriteInt32(_ptrDiseqc, 160, (Int32)diSEqC.Length);//send_message_length
      Marshal.WriteInt32(_ptrDiseqc, 164, (Int32)0);//receive_message_length
      Marshal.WriteInt32(_ptrDiseqc, 168, (Int32)3);//amplitude_attenuation
      Marshal.WriteByte(_ptrDiseqc, 172, 1);//tone_burst_modulated
      Marshal.WriteByte(_ptrDiseqc, 176, (int)DisEqcVersion.DISEQC_VER_1X);
      Marshal.WriteByte(_ptrDiseqc, 180, (int)RxMode.RXMODE_NOREPLY);
      Marshal.WriteByte(_ptrDiseqc, 184, 1);//last_message

      int hr = _propertySet.Set(BdaTunerExtentionProperties, (int)BdaTunerExtension.KSPROPERTY_BDA_DISEQC, _ptrDiseqc, len, _ptrDiseqc, len);
      Log.Log.Info("hauppauge: setdiseqc returned:{0:X}", hr);
      return (hr == 0);
    }

    /// <summary>
    /// gets the diseqc reply
    /// </summary>
    /// <param name="reply">The reply.</param>
    /// <returns>true if succeeded, otherwise false</returns>
    public bool ReadDiSEqCCommand(out byte[] reply)
    {
      reply = new byte[1];
      return false;
    }

    #endregion
  }
}
