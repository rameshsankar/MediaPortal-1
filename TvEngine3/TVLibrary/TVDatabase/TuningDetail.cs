//========================================================================
// This file was generated using the MyGeneration tool in combination
// with the Gentle.NET Business Entity template, $Rev: 965 $
//========================================================================
using System;
using System.Collections;
using Gentle.Common;
using Gentle.Framework;

namespace TvDatabase
{
  /// <summary>
  /// Instances of this class represent the properties and methods of a row in the table <b>TuningDetail</b>.
  /// </summary>
  [TableName("TuningDetail")]
  public class TuningDetail : Persistent
  {
    #region Members
    private bool isChanged;
    [TableColumn("idTuning", NotNull = true), PrimaryKey(AutoGenerated = true)]
    private int idTuning;
    [TableColumn("idChannel", NotNull = true), ForeignKey("Channel", "idChannel")]
    private int idChannel;
    [TableColumn("name", NotNull = true)]
    private string name;
    [TableColumn("provider", NotNull = true)]
    private string provider;
    [TableColumn("channelType", NotNull = true)]
    private int channelType;
    [TableColumn("channelNumber", NotNull = true)]
    private int channelNumber;
    [TableColumn("frequency", NotNull = true)]
    private int frequency;
    [TableColumn("countryId", NotNull = true)]
    private int countryId;
    [TableColumn("isRadio", NotNull = true)]
    private bool isRadio;
    [TableColumn("isTv", NotNull = true)]
    private bool isTv;
    [TableColumn("networkId", NotNull = true)]
    private int networkId;
    [TableColumn("transportId", NotNull = true)]
    private int transportId;
    [TableColumn("serviceId", NotNull = true)]
    private int serviceId;
    [TableColumn("pmtPid", NotNull = true)]
    private int pmtPid;
    [TableColumn("freeToAir", NotNull = true)]
    private bool freeToAir;
    [TableColumn("modulation", NotNull = true)]
    private int modulation;
    [TableColumn("polarisation", NotNull = true)]
    private int polarisation;
    [TableColumn("symbolrate", NotNull = true)]
    private int symbolrate;
    [TableColumn("diseqc", NotNull = true)]
    private int diseqc;
    [TableColumn("switchingFrequency", NotNull = true)]
    private int switchingFrequency;
    [TableColumn("bandwidth", NotNull = true)]
    private int bandwidth;
    [TableColumn("majorChannel", NotNull = true)]
    private int majorChannel;
    [TableColumn("minorChannel", NotNull = true)]
    private int minorChannel;
    [TableColumn("pcrPid", NotNull = true)]
    private int pcrPid;
    [TableColumn("videoSource", NotNull = true)]
    private int videoSource;
    [TableColumn("tuningSource", NotNull = true)]
    private int tuningSource;
    [TableColumn("videoPid", NotNull = true)]
    private int videoPid;
    [TableColumn("audioPid", NotNull = true)]
    private int audioPid;
    [TableColumn("band", NotNull = true)]
    private int band;
    [TableColumn("satIndex", NotNull = true)]
    private int satIndex;
    #endregion

    #region Constructors
    /// <summary> 
    /// Create a new object by specifying all fields (except the auto-generated primary key field). 
    /// </summary> 
    public TuningDetail(int idChannel, string name, string provider, int channelType, int channelNumber, int frequency, int countryId, bool isRadio, bool isTv, int networkId, int transportId, int serviceId, int pmtPid, bool freeToAir, int modulation, int polarisation, int symbolrate, int diseqc, int switchingFrequency, int bandwidth, int majorChannel, int minorChannel, int pcrPid, int videoSource, int tuningSource, int videoPid, int audioPid, int band, int satIndex)
    {
      isChanged = true;
      this.idChannel = idChannel;
      this.name = name;
      this.provider = provider;
      this.channelType = channelType;
      this.channelNumber = channelNumber;
      this.frequency = frequency;
      this.countryId = countryId;
      this.isRadio = isRadio;
      this.isTv = isTv;
      this.networkId = networkId;
      this.transportId = transportId;
      this.serviceId = serviceId;
      this.pmtPid = pmtPid;
      this.freeToAir = freeToAir;
      this.modulation = modulation;
      this.polarisation = polarisation;
      this.symbolrate = symbolrate;
      this.diseqc = diseqc;
      this.switchingFrequency = switchingFrequency;
      this.bandwidth = bandwidth;
      this.majorChannel = majorChannel;
      this.minorChannel = minorChannel;
      this.pcrPid = pcrPid;
      this.videoSource = videoSource;
      this.tuningSource = tuningSource;
      this.audioPid = audioPid;
      this.videoPid = videoPid;
      this.band = band;
      this.satIndex = satIndex;
    }

    /// <summary> 
    /// Create an object from an existing row of data. This will be used by Gentle to 
    /// construct objects from retrieved rows. 
    /// </summary> 
    public TuningDetail(int idTuning, int idChannel, string name, string provider, int channelType, int channelNumber, int frequency, int countryId, bool isRadio, bool isTv, int networkId, int transportId, int serviceId, int pmtPid, bool freeToAir, int modulation, int polarisation, int symbolrate, int diseqc, int switchingFrequency, int bandwidth, int majorChannel, int minorChannel, int pcrPid, int videoSource, int tuningSource, int videoPid, int audioPid, int band, int satIndex)
    {
      this.idTuning = idTuning;
      this.idChannel = idChannel;
      this.name = name;
      this.provider = provider;
      this.channelType = channelType;
      this.channelNumber = channelNumber;
      this.frequency = frequency;
      this.countryId = countryId;
      this.isRadio = isRadio;
      this.isTv = isTv;
      this.networkId = networkId;
      this.transportId = transportId;
      this.serviceId = serviceId;
      this.pmtPid = pmtPid;
      this.freeToAir = freeToAir;
      this.modulation = modulation;
      this.polarisation = polarisation;
      this.symbolrate = symbolrate;
      this.diseqc = diseqc;
      this.switchingFrequency = switchingFrequency;
      this.bandwidth = bandwidth;
      this.majorChannel = majorChannel;
      this.minorChannel = minorChannel;
      this.pcrPid = pcrPid;
      this.videoSource = videoSource;
      this.tuningSource = tuningSource;
      this.audioPid = audioPid;
      this.videoPid = videoPid;
      this.band = band;
      this.satIndex = satIndex;
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Indicates whether the entity is changed and requires saving or not.
    /// </summary>
    public bool IsChanged
    {
      get { return isChanged; }
    }

    /// <summary>
    /// Property relating to database column idTuning
    /// </summary>
    public int IdTuning
    {
      get { return idTuning; }
    }

    /// <summary>
    /// Property relating to database column idChannel
    /// </summary>
    public int IdChannel
    {
      get { return idChannel; }
      set { isChanged |= idChannel != value; idChannel = value; }
    }

    /// <summary>
    /// Property relating to database column name
    /// </summary>
    public string Name
    {
      get { return name; }
      set { isChanged |= name != value; name = value; }
    }

    /// <summary>
    /// Property relating to database column provider
    /// </summary>
    public string Provider
    {
      get { return provider; }
      set { isChanged |= provider != value; provider = value; }
    }

    /// <summary>
    /// Property relating to database column channelType
    /// </summary>
    public int ChannelType
    {
      get { return channelType; }
      set { isChanged |= channelType != value; channelType = value; }
    }
    /// <summary>
    /// Property relating to database column satIndex
    /// </summary>
    public int SatIndex
    {
      get { return satIndex; }
      set { isChanged |= satIndex != value; satIndex = value; }
    }

    /// <summary>
    /// Property relating to database column channelNumber
    /// </summary>
    public int ChannelNumber
    {
      get { return channelNumber; }
      set { isChanged |= channelNumber != value; channelNumber = value; }
    }

    /// <summary>
    /// Property relating to database column frequency
    /// </summary>
    public int Frequency
    {
      get { return frequency; }
      set { isChanged |= frequency != value; frequency = value; }
    }

    /// <summary>
    /// Property relating to database column countryId
    /// </summary>
    public int CountryId
    {
      get { return countryId; }
      set { isChanged |= countryId != value; countryId = value; }
    }

    /// <summary>
    /// Property relating to database column isRadio
    /// </summary>
    public bool IsRadio
    {
      get { return isRadio; }
      set { isChanged |= isRadio != value; isRadio = value; }
    }

    /// <summary>
    /// Property relating to database column isTv
    /// </summary>
    public bool IsTv
    {
      get { return isTv; }
      set { isChanged |= isTv != value; isTv = value; }
    }

    /// <summary>
    /// Property relating to database column networkId
    /// </summary>
    public int NetworkId
    {
      get { return networkId; }
      set { isChanged |= networkId != value; networkId = value; }
    }

    /// <summary>
    /// Property relating to database column transportId
    /// </summary>
    public int TransportId
    {
      get { return transportId; }
      set { isChanged |= transportId != value; transportId = value; }
    }

    /// <summary>
    /// Property relating to database column serviceId
    /// </summary>
    public int ServiceId
    {
      get { return serviceId; }
      set { isChanged |= serviceId != value; serviceId = value; }
    }

    /// <summary>
    /// Property relating to database column pmtPid
    /// </summary>
    public int PmtPid
    {
      get { return pmtPid; }
      set { isChanged |= pmtPid != value; pmtPid = value; }
    }

    /// <summary>
    /// Property relating to database column freeToAir
    /// </summary>
    public bool FreeToAir
    {
      get { return freeToAir; }
      set { isChanged |= freeToAir != value; freeToAir = value; }
    }

    /// <summary>
    /// Property relating to database column modulation
    /// </summary>
    public int Modulation
    {
      get { return modulation; }
      set { isChanged |= modulation != value; modulation = value; }
    }

    /// <summary>
    /// Property relating to database column polarisation
    /// </summary>
    public int Polarisation
    {
      get { return polarisation; }
      set { isChanged |= polarisation != value; polarisation = value; }
    }

    /// <summary>
    /// Property relating to database column symbolrate
    /// </summary>
    public int Symbolrate
    {
      get { return symbolrate; }
      set { isChanged |= symbolrate != value; symbolrate = value; }
    }

    /// <summary>
    /// Property relating to database column diseqc
    /// </summary>
    public int Diseqc
    {
      get { return diseqc; }
      set { isChanged |= diseqc != value; diseqc = value; }
    }

    /// <summary>
    /// Property relating to database column switchingFrequency
    /// </summary>
    public int SwitchingFrequency
    {
      get { return switchingFrequency; }
      set { isChanged |= switchingFrequency != value; switchingFrequency = value; }
    }

    /// <summary>
    /// Property relating to database column bandwidth
    /// </summary>
    public int Bandwidth
    {
      get { return bandwidth; }
      set { isChanged |= bandwidth != value; bandwidth = value; }
    }

    /// <summary>
    /// Property relating to database column majorChannel
    /// </summary>
    public int MajorChannel
    {
      get { return majorChannel; }
      set { isChanged |= majorChannel != value; majorChannel = value; }
    }

    /// <summary>
    /// Property relating to database column minorChannel
    /// </summary>
    public int MinorChannel
    {
      get { return minorChannel; }
      set { isChanged |= minorChannel != value; minorChannel = value; }
    }

    /// <summary>
    /// Property relating to database column pcrPid
    /// </summary>
    public int PcrPid
    {
      get { return pcrPid; }
      set { isChanged |= pcrPid != value; pcrPid = value; }
    }

    /// <summary>
    /// Property relating to database column videoSource
    /// </summary>
    public int VideoSource
    {
      get { return videoSource; }
      set { isChanged |= videoSource != value; videoSource = value; }
    }

    /// <summary>
    /// Property relating to database column tuningSource
    /// </summary>
    public int TuningSource
    {
      get { return tuningSource; }
      set { isChanged |= tuningSource != value; tuningSource = value; }
    }

    /// <summary>
    /// Property relating to database column videoPid
    /// </summary>
    public int VideoPid
    {
      get { return videoPid; }
      set { isChanged |= videoPid != value; videoPid = value; }
    }
    /// <summary>
    /// Property relating to database column audioPid
    /// </summary>
    public int AudioPid
    {
      get { return audioPid; }
      set { isChanged |= audioPid != value; audioPid = value; }
    }
    /// <summary>
    /// Property relating to database column band
    /// </summary>
    public int Band
    {
      get { return band; }
      set { isChanged |= band != value; band = value; }
    }
    #endregion

    #region Storage and Retrieval

    /// <summary>
    /// Static method to retrieve all instances that are stored in the database in one call
    /// </summary>
    public static IList ListAll()
    {
      return Broker.RetrieveList(typeof(TuningDetail));
    }

    /// <summary>
    /// Retrieves an entity given it's id.
    /// </summary>
    public static TuningDetail Retrieve(int id)
    {
      // Return null if id is smaller than seed and/or increment for autokey
      if (id < 1)
      {
        return null;
      }
      Key key = new Key(typeof(TuningDetail), true, "idTuning", id);
      return Broker.RetrieveInstance(typeof(TuningDetail), key) as TuningDetail;
    }

    /// <summary>
    /// Retrieves an entity given it's id, using Gentle.Framework.Key class.
    /// This allows retrieval based on multi-column keys.
    /// </summary>
    public static TuningDetail Retrieve(Key key)
    {
      return Broker.RetrieveInstance(typeof(TuningDetail), key) as TuningDetail;
    }

    /// <summary>
    /// Persists the entity if it was never persisted or was changed.
    /// </summary>
    public override void Persist()
    {
      if (IsChanged || !IsPersisted)
      {
        base.Persist();
        isChanged = false;
      }
    }

    #endregion


    #region Relations
    /// <summary>
    ///
    /// </summary>
    public Channel ReferencedChannel()
    {
      return Channel.Retrieve(IdChannel);
    }
    #endregion
  }
}
