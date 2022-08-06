using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace CommonObjects.DataModels.VoiceData
{
    internal class VoiceHostData : IVoiceData
    {
        public int Length { get => EndPoints.Count * HostDataSize; }
        public List<IPEndPoint> EndPoints { get; } = new List<IPEndPoint>();
        private readonly int HostDataSize = Marshal.SizeOf<HostDataFormat>();

        private struct HostDataFormat
        {
            public long ipv4;
            public short port;
        }

        public int CopyFrom(byte[] buffer, int offset, int count)
        {
            if (buffer == null) throw new ArgumentNullException("buffer is null");
            if (buffer.Length < offset + count)
                count = buffer.Length - offset;

            if (count % HostDataSize != 0) throw new ArgumentException("host data count should be a multiple of " + HostDataSize.ToString());

            IntPtr ptr = Marshal.AllocHGlobal(HostDataSize);
            for (int startIndex = offset; startIndex < offset + count; startIndex += 6)
            {
                Marshal.Copy(buffer, startIndex, ptr, HostDataSize);
                var data = Marshal.PtrToStructure<HostDataFormat>(ptr);
                EndPoints.Add(new IPEndPoint(new IPAddress(data.ipv4), data.port));
            }
            Marshal.FreeHGlobal(ptr);

            return EndPoints.Count;
        }

        public int CopyTo(byte[] buffer, int offset)
        {
            if (buffer == null) throw new ArgumentNullException("buffer is null");

            if (buffer.Length < offset + Length)
                throw new OutOfMemoryException("Buffer size is smaller than target data length");

            var ptr = Marshal.AllocHGlobal(HostDataSize);
            HostDataFormat data;
            for (int i = 0; i < EndPoints.Count; ++i)
            {
                data.ipv4 = BitConverter.ToUInt32(EndPoints[i].Address.GetAddressBytes(), 0);
                data.port = (short)EndPoints[i].Port;

                Marshal.StructureToPtr(data, ptr, true);
                Marshal.Copy(ptr, buffer, offset + (i * HostDataSize), HostDataSize);
            }
            Marshal.FreeHGlobal(ptr);

            return Length;
        }

        public void Dispose()
        {
            EndPoints.Clear();
        }
    }
}
