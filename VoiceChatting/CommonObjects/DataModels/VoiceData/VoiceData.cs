using CommonObjects.MemoryPool;
using System;
using System.Runtime.InteropServices;

namespace CommonObjects.DataModels.VoiceData.Model
{
    internal struct VoiceDataHeader
    {
        public int Command { get; set; }
        public int Length { get; set; }

        public void CopyFrom(VoiceDataHeader from)
        {
            Command = from.Command;
            Length = from.Length;
        }
    }

    internal class VoiceData : IDisposable
    {
        public VoiceDataHeader Header;
        public byte[] Data
        {
            get
            {
                if (memoryPoolIndex == -1) return null;
                return memoryPool[memoryPoolIndex];
            }
        }

        private ByteMemoryPool memoryPool;
        private int memoryPoolIndex;

        public VoiceData(ByteMemoryPool memoryPool)
        {
            Header = new VoiceDataHeader();
            this.memoryPool = memoryPool;
            memoryPoolIndex = memoryPool.LockBuffer();
        }

        public static VoiceData FromBytes(ByteMemoryPool memoryPool, byte[] data)
        {
            // Convert header data as structure
            int headerSize = Marshal.SizeOf(typeof(VoiceDataHeader));
            IntPtr ptr = Marshal.AllocHGlobal(headerSize);
            Marshal.Copy(data, 0, ptr, headerSize);
            VoiceDataHeader header = Marshal.PtrToStructure<VoiceDataHeader>(ptr);
            Marshal.FreeHGlobal(ptr);

            VoiceData voiceData = new VoiceData(memoryPool);
            voiceData.Header.CopyFrom(header);

            if (memoryPool.Size < header.Length)
                throw new Exception("Data size is logger than memory pool size");
            if (header.Length + headerSize != data.Length)
                throw new Exception("Data size mismatch");

            Array.Copy(data, headerSize, voiceData.Data, 0, voiceData.Header.Length);
            return voiceData;
        }

        public int ToBytes(ref byte[] buffer)
        {
            int headerSize = Marshal.SizeOf(typeof(VoiceDataHeader));
            if (buffer.Length < headerSize + Header.Length)
                throw new Exception("Buffer size is too small, Excepcted : " + headerSize + Header.Length + " Real : " + buffer.Length);

            IntPtr ptr = Marshal.AllocHGlobal(headerSize);
            Marshal.StructureToPtr(Header, ptr, true);
            Marshal.Copy(ptr, buffer, 0, headerSize);
            Marshal.FreeHGlobal(ptr);

            Array.Copy(Data, 0, buffer, headerSize, Header.Length);
            return headerSize + Header.Length;
        }

        public int CopyVoiceData(byte[] datas)
        {
            if (Data.Length < datas.Length)
                throw new Exception("Data is too long");

            Array.Copy(datas, 0, Data, 0, datas.Length);
            Header.Length = datas.Length;
            return datas.Length;
        }

        public int CopyVoiceData(byte[] datas, int length)
        {
            if (Data.Length < length)
                throw new Exception("Data is too long");

            Array.Copy(datas, 0, Data, 0, length);
            Header.Length = length;
            return length;
        }

        public void Dispose()
        {
            memoryPool.UnlockBuffer(memoryPoolIndex);
        }
    }
}
