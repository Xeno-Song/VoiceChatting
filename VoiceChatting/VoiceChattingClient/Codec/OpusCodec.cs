using OpusDotNet;
using System;
using System.Diagnostics;
using CommonObjects.MemoryPool;
using CommonObjects;

namespace VoiceChattingClient.Codec
{
    internal class OpusCodec
    {
        private OpusEncoder opusEncoder = null;
        private readonly int bufferSize;

        public OpusCodec()
        {
            int sampleRate = 48000;
            int channels = 1;
            bufferSize = (int)(10 * sampleRate / 1000 * channels * 2);

            opusEncoder = new OpusEncoder(Application.Audio, sampleRate, channels);
            opusEncoder.VBR = true;
        }

        public int Encode(byte[] rawDatas, int rawCount, byte[] encodedDatas)
        {
            var buffer = Common.BufferManager.TakeBuffer(bufferSize);
            int dataLength = 0;

            try
            {
                dataLength = opusEncoder.Encode(rawDatas, 960, buffer, bufferSize);
                Array.Copy(encodedDatas, buffer, dataLength);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                Common.BufferManager.ReturnBuffer(buffer);
            }

            return dataLength;
        }
    }
}
