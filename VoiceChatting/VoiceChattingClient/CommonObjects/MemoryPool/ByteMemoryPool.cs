using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChattingClient.CommonObjects.MemoryPool
{
    public class ByteMemoryPoolIndex
    {
        public byte[] Buffer { get; set; }
        public bool IsUsing { get; private set; }
        private object useLock = new object();
        private readonly int initialSize;

        public ByteMemoryPoolIndex(int size)
        {
            initialSize = size;
            Buffer = new byte[size];
            ClearBuffer();
        }

        public bool LockMemory()
        {
            lock (useLock)
            {
                if (IsUsing == false)
                {
                    IsUsing = true;
                    return true;
                }

                return false;
            }
        }
        public void UnlockMemory()
        {
            lock (useLock)
            {
                if (Buffer.Length != initialSize)
                    Buffer = new byte[initialSize];

                ClearBuffer();
                IsUsing = false;
            }
        }
        private void ClearBuffer()
        {
            Array.Clear(Buffer, 0, Buffer.Length);
        }
    }

    public class ByteMemoryPool
    {
        private List<ByteMemoryPoolIndex> memoryPool;
        public byte[] this[int bufferId]
        {
            get
            {
                if (!memoryPool[bufferId].IsUsing) return null;
                return memoryPool[bufferId].Buffer;
            }
        }
        public int Size { get; private set; }
        public int Count { get; private set; }

        public ByteMemoryPool(int size, int count)
        {
            memoryPool = new List<ByteMemoryPoolIndex>();

            for (int i = 0; i < count; ++i)
                memoryPool.Add(new ByteMemoryPoolIndex(size));

            Size = size;
            Count = count;
        }

        public int LockBuffer()
        {
            for (int i = 0; i < memoryPool.Count; ++i)
            {
                var index = memoryPool[i];
                if (!index.IsUsing)
                {
                    index.LockMemory();
                    return i;
                }
            }

            return -1;
        }

        public void UnlockBuffer(int bufferId)
        {
            if (bufferId >= memoryPool.Count) return;
            memoryPool[bufferId].UnlockMemory();
        }
    }
}
