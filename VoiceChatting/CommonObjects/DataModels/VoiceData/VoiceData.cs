using CommonObjects.MemoryPool;
using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;

namespace CommonObjects.DataModels.VoiceData
{
    internal class VoiceData
    {
        public VoiceDataHeader Header { get; set; }
        public IVoiceData Data { get; set; }
        
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

        public bool Parse(byte[] datas, int offset, int count, ByteMemoryPool byteMemoryPool)
        {
            Header = VoiceDataHeader.Parse(datas, offset);
            var headerLength = VoiceDataHeader.HeaderLength;

            switch (Header.Command)
            {
                case 0:
                    {
                        Data = new VoiceWaveData(Common.BufferManager.TakeBuffer(count - headerLength));
                        Data.CopyFrom(datas, offset + headerLength, count - headerLength);
                        break;
                    }
                case 1:
                    {
                        Data = new VoiceHostData();
                        Data.CopyFrom(datas, offset + headerLength, count - headerLength);
                    }
                    break;
                default: return false;
            }

            return true;
        }
    }
}
