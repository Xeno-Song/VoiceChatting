using CommonObjects.MemoryPool;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel.Channels;
using System.Text;

namespace CommonObjects.DataModels.VoiceData
{
    internal class VoiceData : IDisposable
    {
        public VoiceDataHeader Header { get; set; }
        public IVoiceData Data { get; set; }
        public IPEndPoint Sender { get; set; }
        
        /// <summary>
        /// <br>If data type is wave data, return WaveData</br>
        /// <br>else return <see langword="null"/></br>
        /// </summary>
        public VoiceWaveData WaveData {
            get
            {
                if (Data is VoiceWaveData)
                    return Data as VoiceWaveData;
                return null;
            }
        }

        /// <summary>
        /// <br>If data type is host data, return HostData</br>
        /// <br>else return <see langword="null"/></br>
        /// </summary>
        public VoiceHostData HostData
        {
            get
            {
                if (Data is VoiceHostData)
                    return Data as VoiceHostData;
                return null;
            }
        }

        public void Dispose()
        {
            Header = null;
            Sender = null;
            Data.Dispose();
        }

        public static VoiceData Parse(byte[] datas, int offset, int count)
        {
            VoiceData voiceData = new VoiceData();
            voiceData.Header = VoiceDataHeader.Parse(datas, offset);
            var headerLength = VoiceDataHeader.HeaderLength;

            switch (voiceData.Header.Command)
            {
                case 0:
                    {
                        voiceData.Data = new VoiceWaveData(Common.BufferManager.TakeBuffer(count - headerLength));
                        voiceData.Data.CopyFrom(datas, offset + headerLength, count - headerLength);
                        break;
                    }
                case 1:
                    {
                        voiceData.Data = new VoiceHostData();
                        voiceData.Data.CopyFrom(datas, offset + headerLength, count - headerLength);
                    }
                    break;
                default:
                    throw new NotSupportedException("Header command number is invalid");
            }

            return voiceData;
        }

        public int ToBytes(byte[] buffer, int length)
        {
            if (VoiceDataHeader.HeaderLength + Data.Length > length)
                throw new OutOfMemoryException("Buffer size is smaller than data size");

            Header.CopyTo(ref buffer, 0);
            Data.CopyTo(buffer, VoiceDataHeader.HeaderLength);

            return VoiceDataHeader.HeaderLength + Data.Length;
        }
    }
}
