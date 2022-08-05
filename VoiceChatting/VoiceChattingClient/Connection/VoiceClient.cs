using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonObjects.MemoryPool;
using CommonObjects.DataModels.VoiceData.Model;

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
        public event EventHandler<VoiceData> OnVoiceDataReceived;

        private UdpClient socket = null;
        private Thread socketReceiveThread;
        private bool isClosing = false;
        private List<IPEndPoint> clientList = new List<IPEndPoint>();
        
        
        private ByteMemoryPool byteMemoryPool;
        private int sendBufferMemoryPoolIndex;
        private object sendLock = new object();
        private byte[] SendBuffer { get => byteMemoryPool[sendBufferMemoryPoolIndex]; }


        public VoiceClient(string hostname, int port)
        {
            HostName = hostname;
            Port = port;

            byteMemoryPool = new ByteMemoryPool(2048, 10);
            sendBufferMemoryPoolIndex = byteMemoryPool.LockBuffer();

            socket = new UdpClient();
            socketReceiveThread = new Thread(VoiceDataReceiveThread);
        }

        public void Connect()
        {
            socket.Connect(HostName, Port);
            socketReceiveThread.Start();

            var hostEntry = Dns.GetHostEntry(HostName);
            if (hostEntry.AddressList.Length != 0)
            {
                clientList.Add(new IPEndPoint(IPAddress.Parse(HostName), Port));
            }
        }

        public void Bind()
        {
            if (socket != null)
            {
                Disconnect();
                socket.Dispose();
            }

            socket = new UdpClient(Port);
            socketReceiveThread.Start();
        }

        public void Disconnect()
        {
            isClosing = true;

            if (socketReceiveThread != null && socketReceiveThread.IsAlive)
                socketReceiveThread.Join();

            socket.Close();
        }

        public void SendVoiceData(byte[] datas)
        {
            lock(sendLock)
            {
                VoiceData voiceData = new VoiceData(byteMemoryPool);
                voiceData.CopyVoiceData(datas);

                byte[] sendBuffer = SendBuffer;
                int dataSize = voiceData.ToBytes(buffer: ref sendBuffer);
                socket.SendAsync(sendBuffer, dataSize).Wait();
                voiceData.Dispose();
            }
        }

        private void VoiceDataReceiveThread()
        {
            Task<UdpReceiveResult> receiveTask = null;

            while (true)
            {
                if (receiveTask == null)
                    receiveTask = socket.ReceiveAsync();
                if (!receiveTask.Wait(10)) continue;

                if (!clientList.Contains(receiveTask.Result.RemoteEndPoint))
                    clientList.Add(receiveTask.Result.RemoteEndPoint);

                Debug.WriteLine("UDP Packet Received");

                var voiceData = VoiceData.FromBytes(byteMemoryPool, receiveTask.Result.Buffer);
                OnVoiceDataReceived?.Invoke(this, voiceData);
                voiceData.Dispose();

                receiveTask.Dispose();
                receiveTask = null;
            }
        }
    }
}
