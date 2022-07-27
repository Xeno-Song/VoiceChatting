using OpusDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceChattingClient.CommonObjects.MemoryPool;

namespace VoiceChattingClient.Codec
{
    internal class VoiceEncoder
    {
        private OpusEncoder opusEncoder = null;
        private ByteMemoryPool memoryPool = null;
        private readonly int bufferSize;

        public VoiceEncoder()
        {
            int sampleRate = 48000;
            int channels = 1;
            bufferSize = (int)(sampleRate * (10 / (double)1000) * channels * 2);

            opusEncoder = new OpusEncoder(Application.VoIP, sampleRate, channels);
            memoryPool = new ByteMemoryPool(bufferSize, 16);
        }

        public int Encode(byte[] rawDatas, byte[] encodedDatas)
        {
            int bufferId = memoryPool.LockBuffer();
            if (bufferId == -1) return -1;

            int dataLength = opusEncoder.Encode(rawDatas, rawDatas.Length, memoryPool[bufferId], bufferSize);
            Array.Copy(encodedDatas, memoryPool[bufferId], dataLength);

            return dataLength;
        }
    }
}
