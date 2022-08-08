using System;
using System.Collections.Generic;
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

        public void Parse(byte[] datas, int offset, int count)
        {
            Header = VoiceDataHeader.Parse(datas, offset);
            var headerLength = VoiceDataHeader.HeaderLength;

            switch (Header.Command)
            {

            }
        }
    }
}
