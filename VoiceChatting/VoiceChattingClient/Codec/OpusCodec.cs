using OpusDotNet;
using System;
using System.Diagnostics;
using VoiceChattingClient.CommonObjects.MemoryPool;

namespace VoiceChattingClient.Codec
{
    internal class OpusCodec
    {
        private OpusEncoder opusEncoder = null;
        private ByteMemoryPool memoryPool = null;
        private readonly int bufferSize;

        public OpusCodec()
        {
            int sampleRate = 48000;
            int channels = 1;
            bufferSize = (int)(10 * sampleRate / 1000 * channels * 2);

            opusEncoder = new OpusEncoder(Application.Audio, sampleRate, channels);
            opusEncoder.VBR = true;
            memoryPool = new ByteMemoryPool(bufferSize, 16);
        }

        public int Encode(byte[] rawDatas, int rawCount, byte[] encodedDatas)
        {
            int bufferId = memoryPool.LockBuffer();
            if (bufferId == -1) return -1;
            int dataLength = 0;

            try
            {
                dataLength = opusEncoder.Encode(rawDatas, 960, memoryPool[bufferId], bufferSize);
                Array.Copy(encodedDatas, memoryPool[bufferId], dataLength);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                memoryPool.UnlockBuffer(bufferId);
            }

            return dataLength;
        }
    }
}
