using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceChattingClient.CommonObjects.MemoryPool
{
    internal class ByteMemoryPool
    {
        private List<ByteMemoryPoolIndex> memoryPool;

        private class ByteMemoryPoolIndex
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

        public ByteMemoryPool(int size, int count)
        {
            memoryPool = new List<ByteMemoryPoolIndex>();

            for (int i = 0; i < count; ++i)
                memoryPool.Add(new ByteMemoryPoolIndex(size));
        }

        public int LockBuffer()
        {
            var memoryIndex = memoryPool.First((x) =>
            {
                if (x.IsUsing) return false;
                return x.LockMemory();
            });
            if (memoryIndex == null) return -1;
            return memoryPool.IndexOf(memoryIndex);
        }

        public void UnlockBuffer(int bufferId)
        {
            if (bufferId >= memoryPool.Count) return;
            memoryPool[bufferId].UnlockMemory();
        }
    }
}
