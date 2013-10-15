/* 
 *  Copyright (C) 2005-2013 Team MediaPortal
 *  http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */
#pragma once
#include "..\..\shared\stdafx.h"
#include <streams.h>
#include <initguid.h>
#include <Windows.h>
#include <map>
#include <vector>
#include "IMuxInputPin.h"
#include "IStreamMultiplexer.h"
#include "MuxInputPin.h"
#include "TsOutputPin.h"

using namespace std;

class CTsMuxer;
class CTsMuxerFilter;

// {511d13f0-8a56-42fa-b151-b72a325cf71a}
DEFINE_GUID(CLSID_TS_MUXER, 0x511d13f0, 0x8a56, 0x42fa, 0xb1, 0x51, 0xb7, 0x2a, 0x32, 0x5c, 0xf7, 0x1a);

// {8533d2d1-1be1-4262-b70a-432df592b903}
DEFINE_GUID(IID_ITS_MUXER, 0x8533d2d1, 0x1be1, 0x4262, 0xb7, 0xa, 0x43, 0x2d, 0xf5, 0x92, 0xb9, 0x3);

DECLARE_INTERFACE_(ITsMuxer, IUnknown)
{
  STDMETHOD(SetActiveComponents)(THIS_ bool video, bool audio, bool teletext)PURE;
};

#define TS_PACKET_LENGTH 188
#define SERVICE_NAME_LENGTH 20


struct StreamInfo
{
  byte pinId;
  byte originalStreamId;
  unsigned short originalPid;

  bool isIgnored;
  bool isCompatible;
  DWORD prevReceiveTickCount;

  unsigned short pid;
  byte streamId;
  byte streamType;
  byte continuityCounter;

  byte* pmtDescriptorBytes;
  unsigned short pmtDescriptorLength;
};

struct ProgramStreamInfo
{
  byte pinId;
  bool isCompatible;
  byte videoBound;
  byte audioBound;
  byte currentMapVersion;
};

struct TransportStreamInfo
{
  byte pinId;
  bool isCompatible;
  unsigned short transportStreamId;
  unsigned short serviceId;
  unsigned short pmtPid;
  unsigned short pcrPid;
  byte streamCount;
  byte patVersion;
  byte patContinuityCounter;
  byte pmtVersion;
  byte pmtContinuityCounter;
};


// Filter class.
class CTsMuxerFilter : public CBaseFilter
{
  public:
    CTsMuxerFilter(CTsMuxer* tsMuxer, LPUNKNOWN unk, CCritSec* filterLock, CCritSec* receiveLock, HRESULT* hr);
    ~CTsMuxerFilter();

    CBasePin* GetPin(int n);
    HRESULT AddPin();
    int GetPinCount();
    HRESULT Deliver(PBYTE data, long dataLength);

    STDMETHODIMP Pause();
    STDMETHODIMP Run(REFERENCE_TIME startTime);
    STDMETHODIMP Stop();

    static void __cdecl StreamingMonitorThreadFunction(void* arg);

  private:
    IStreamMultiplexer* const m_tsMuxer;
    CTsOutputPin* m_outputPin;          // MPEG 2 transport stream output pin
    vector<CMuxInputPin*> m_inputPins;  // input pins
    CCritSec* m_receiveLock;            // sample receive lock

    HANDLE m_streamingMonitorThread;
    HANDLE m_streamingMonitorThreadStopEvent;
};


// Muxer class.
class CTsMuxer : public CUnknown, public IStreamMultiplexer, public ITsMuxer
{
  friend class CTsMuxerFilter;
  friend class CTsOutputPin;

  public:
    DECLARE_IUNKNOWN

    CTsMuxer(LPUNKNOWN unk, HRESULT* hr);
    ~CTsMuxer();

    static CUnknown* WINAPI CreateInstance(LPUNKNOWN unk, HRESULT* hr);

    HRESULT BreakConnect(IMuxInputPin* pin);
    HRESULT CompleteConnect(IMuxInputPin* pin);
    bool IsStarted();
    HRESULT Receive(IMuxInputPin* pin, PBYTE data, long dataLength, REFERENCE_TIME dataStartTime);
    HRESULT Reset();
    HRESULT StreamTypeChange(IMuxInputPin* pin, int oldStreamType, int newStreamType);
    STDMETHODIMP SetActiveComponents(bool video, bool audio, bool teletext);

  private:
    STDMETHODIMP NonDelegatingQueryInterface(REFIID iid, void** ppv);
    bool CanDeliver();
    HRESULT ReceiveTransportStream(IMuxInputPin* pin, PBYTE data, long dataLength, REFERENCE_TIME dataStartTime);
    HRESULT ReceiveProgramOrSystemStream(IMuxInputPin* pin, PBYTE data, long dataLength, REFERENCE_TIME dataStartTime);
    HRESULT ReadProgramAssociationTable(PBYTE data, long dataLength, TransportStreamInfo* info);
    HRESULT ReadProgramMapTable(PBYTE data, long dataLength, TransportStreamInfo* info);
    HRESULT ReadProgramOrSystemPack(PBYTE data, long dataLength, ProgramStreamInfo* info, bool isFirstReceive, int* length, REFERENCE_TIME* systemClockReference);
    HRESULT ReadProgramOrSystemHeader(PBYTE data, long dataLength, ProgramStreamInfo* info, bool isFirstReceive);
    HRESULT ReadProgramStreamMap(PBYTE data, long dataLength, ProgramStreamInfo* info);
    HRESULT ReadVideoStreamInfo(PBYTE data, long dataLength, StreamInfo* info);
    HRESULT ReadAudioStreamInfo(PBYTE data, long dataLength, StreamInfo* info);
    HRESULT UpdatePat();
    HRESULT UpdatePmt();
    HRESULT UpdateSdt();
    HRESULT WrapVbiTeletextData(StreamInfo* info, PBYTE inputData, long inputDataLength, REFERENCE_TIME systemClockReference, PBYTE* outputData, long* outputDataLength);
    HRESULT WrapElementaryStreamData(StreamInfo* info, PBYTE inputData, long inputDataLength, REFERENCE_TIME systemClockReference, PBYTE* outputData, long* outputDataLength);
    HRESULT WrapPacketisedElementaryStreamData(StreamInfo* info, PBYTE inputData, long inputDataLength, REFERENCE_TIME systemClockReference, PBYTE* outputData, long* outputDataLength);
    HRESULT DeliverTransportStreamData(PBYTE inputData, long inputDataLength);

    CTsMuxerFilter* m_filter;
    CCritSec m_filterLock;                  // filter control lock
    CCritSec m_receiveLock;                 // sample receive lock

    bool m_isStarted;
    bool m_isVideoActive;
    bool m_isAudioActive;
    bool m_isTeletextActive;

    byte m_patPacket[TS_PACKET_LENGTH];
    byte m_patContinuityCounter;

    byte m_pmtPacket[TS_PACKET_LENGTH];
    byte m_pmtContinuityCounter;
    unsigned short m_pmtPid;
    byte m_pmtVersion;

    byte m_sdtPacket[TS_PACKET_LENGTH];
    byte m_sdtContinuityCounter;
    byte m_sdtVersion;
    char m_serviceName[SERVICE_NAME_LENGTH + 1];
    byte m_serviceType;
    DWORD m_sdtResetTime;

    int m_packetCounter;
    int m_pcrPid;
    unsigned short m_nextStreamPid;
    byte m_nextVideoStreamId;
    byte m_nextAudioStreamId;

    map<unsigned int, StreamInfo*> m_streamInfo;            // key = (original PID << 16) | (original stream ID << 8) | pin ID
    map<byte, ProgramStreamInfo*> m_programStreamInfo;      // key = pin ID
    map<byte, TransportStreamInfo*> m_transportStreamInfo;  // key = pin ID
};