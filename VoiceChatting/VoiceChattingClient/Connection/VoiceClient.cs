using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VoiceChattingClient.Connection
{
    internal struct VoiceDataHeader
    {
        public int Command { get; set; }
        public int Length { get; set; }
    }

    internal struct VoiceDataFormat
    {
        public VoiceDataHeader Header;
        public byte[] Data;
    }

    internal class VoiceClient
    {
        public string HostName { get; private set; }
        public int Port { get; private set; }
        public event EventHandler<byte[]> OnVoiceDataReceived;
        private UdpClient socket = null;
        private Thread socketReceiveThread;
        private bool isClosing = false;

        public VoiceClient(string hostname, int port)
        {
            HostName = hostname;
            Port = port;

            socket = new UdpClient();
            socketReceiveThread = new Thread(VoiceDataReceiveThread);
        }

        public void Connect()
        {
            socket.Connect(HostName, Port);
            socketReceiveThread.Start();
        }

        public void Bind()
        {
            if (socket != null)
            {
                Disconnect();
                socket.Dispose();
            }

            socket = new UdpClient(Port);
        }

        public void Disconnect()
        {
            isClosing = true;
            socketReceiveThread.Join();
            socket.Close();
        }

        public void SendVoiceData(byte[] datas)
        {
            var header = new VoiceDataHeader
            {
                Command = 0,
                Length = datas.Length
            };
            int headerSize = Marshal.SizeOf(typeof(VoiceDataHeader));

            byte[] data = new byte[headerSize + datas.Length];
            
            IntPtr ptr = Marshal.AllocHGlobal(headerSize);
            Marshal.StructureToPtr(header, ptr, true);
            Marshal.Copy(ptr, data, 0, headerSize);
            Marshal.FreeHGlobal(ptr);

            Array.Copy(datas, 0, data, headerSize, datas.Length);
        }

        private void VoiceDataReceiveThread()
        {
            Task<UdpReceiveResult> receiveTask = null;

            while (true)
            {
                if (receiveTask == null)
                    receiveTask = socket.ReceiveAsync();
                if (!receiveTask.Wait(10)) continue;

                int headerSize = Marshal.SizeOf(typeof(VoiceDataHeader));
                IntPtr ptr = Marshal.AllocHGlobal(headerSize);
                Marshal.Copy(receiveTask.Result.Buffer, 0, ptr, headerSize);
                VoiceDataHeader header = Marshal.PtrToStructure<VoiceDataHeader>(ptr);
                Marshal.FreeHGlobal(ptr);

                VoiceDataFormat data = new VoiceDataFormat();
                data.Header = header;
                data.Data = new byte[header.Length];
                receiveTask.Result.Buffer.CopyTo(data.Data, headerSize);

                OnVoiceDataReceived?.Invoke(this, receiveTask.Result.Buffer);
            }
        }
    }
}
