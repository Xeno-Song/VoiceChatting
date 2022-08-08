using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CommonObjects.DataModels.VoiceData
{
    internal class VoiceDataHeader
    {
        public static int HeaderLength { get => Marshal.SizeOf(typeof(VoiceDataHeader)); }

        /// <summary>
        /// Data type command
        /// </summary>
        public short Command { get; set; }
        /// <summary>
        /// Data Length
        /// </summary>
        public short Length { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public int Reserved_1 { get; set; }
        /// <summary>
        /// Reserved
        /// </summary>
        public int Reserved_2 { get; set; }

        public void CopyFrom(VoiceDataHeader from)
        {
            Command = from.Command;
            Length = from.Length;
        }

        /// <summary>
        /// Parse header data from byte array
        /// </summary>
        /// <param name="datas"></param>
        /// <returns>Return parsed <see cref="VoiceDataHeader"/> object</returns>
        /// <exception cref="ArgumentException" />
        public static VoiceDataHeader Parse(byte[] datas, int startIndex = 0)
        {
            int headerSize = Marshal.SizeOf(typeof(VoiceDataHeader));

            // Check data size is bigger than expected header size
            if (datas.Length - startIndex < headerSize)
                throw new ArgumentException("Buffer size is smaller than expected header size");

            // Create header object using received data
            IntPtr ptr = Marshal.AllocHGlobal(headerSize);
            Marshal.Copy(datas, 0, ptr, headerSize);
            VoiceDataHeader header = Marshal.PtrToStructure<VoiceDataHeader>(ptr);
            Marshal.FreeHGlobal(ptr);

            return header;
        }

        /// <summary>
        /// Copy voice data header datas to byte array
        /// </summary>
        /// <param name="buffer">Target buffer</param>
        /// <param name="offset">Start index for copy</param>
        /// <returns>Return VoiceDataHeader size</returns>
        /// <exception cref="ArgumentException" />
        public int CopyTo(ref byte[] buffer, int offset)
        {
            int headerSize = Marshal.SizeOf(typeof(VoiceDataHeader));

            // Check buffer size is bigger than header size
            if (buffer.Length - offset < headerSize)
                throw new ArgumentException("Buffer size is smaller than header size. Cannot copy datas to buffer");

            // Copy header data to buffer from offset after converting to byte data
            IntPtr ptr = Marshal.AllocHGlobal(headerSize);
            Marshal.StructureToPtr(this, ptr, true);
            Marshal.Copy(ptr, buffer, offset, headerSize);
            Marshal.FreeHGlobal(ptr);

            return headerSize;
        }
    }
}
