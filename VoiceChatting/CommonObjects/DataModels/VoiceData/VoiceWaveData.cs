using System;
using System.Collections.Generic;
using System.Text;

namespace CommonObjects.DataModels.VoiceData
{
    internal class VoiceWaveData : IVoiceData
    {
        public int Length { get; private set; } = 0;
        public byte[] WaveData { get; } = null;

        public VoiceWaveData(byte[] buffer)
        {
            WaveData = buffer;
        }

        public int CopyFrom(byte[] buffer, int offset, int count)
        {
            if (WaveData == null || buffer == null) return -1;
            if (WaveData.Length < count) return -1;
            if (buffer.Length < offset + count)
                count = buffer.Length - offset;

            Array.Copy(buffer, offset, WaveData, 0, count);
            Length = count;
            return count;
        }

        public int CopyTo(byte[] buffer, int offset)
        {
            if (WaveData == null) throw new NullReferenceException("WaveData is not initialized");
            if (buffer == null) throw new ArgumentNullException("buffer is null");

            if (WaveData.Length < buffer.Length - offset)
                throw new OutOfMemoryException("Buffer length is smaller than wave data size");

            Array.Copy(WaveData, 0, buffer, offset, Length);
            return Length;
        }

        public void Dispose()
        {
        }
    }
}
