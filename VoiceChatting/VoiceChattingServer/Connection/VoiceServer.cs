using CommonObjects.DataModels.VoiceData.Model;
using CommonObjects.MemoryPool;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VoiceChattingServer.Connection
{
    internal class VoiceServer
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
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public VoiceServer(string hostname, int port)
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
            isClosing = false;

            socket = new UdpClient(Port);
            socketReceiveThread.Start();
        }

        public void Disconnect()
        {
            isClosing = true;

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }

            if (socketReceiveThread != null && socketReceiveThread.IsAlive)
                socketReceiveThread.Join();

            socket.Close();
        }

        public void SendVoiceData(byte[] datas)
        {
            lock (sendLock)
            {
                VoiceData voiceData = new VoiceData(byteMemoryPool);
                voiceData.CopyVoiceData(datas);

                byte[] sendBuffer = SendBuffer;
                int dataSize = voiceData.ToBytes(buffer: ref sendBuffer);
                socket.SendAsync(sendBuffer, dataSize).Wait();
                voiceData.Dispose();
            }
        }

        public void SendVoiceData(IPEndPoint endPoint, byte[] datas, int length)
        {
            lock (sendLock)
            {
                VoiceData voiceData = new VoiceData(byteMemoryPool);
                voiceData.CopyVoiceData(datas, length);

                byte[] sendBuffer = SendBuffer;
                int dataSize = voiceData.ToBytes(buffer: ref sendBuffer);
                socket.SendAsync(sendBuffer, dataSize, endPoint).Wait();
                voiceData.Dispose();
            }
        }

        private void VoiceDataReceiveThread()
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            try
            {
                using (cancellationToken.Register(Thread.CurrentThread.Abort))
                {
                    while (!isClosing)
                    {
                        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, Port);
                        var buffer = socket.Receive(ref endPoint);
                        var voiceData = VoiceData.FromBytes(byteMemoryPool, buffer);
                        OnVoiceDataReceived?.Invoke(endPoint, voiceData);
                        voiceData.Dispose();
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine("Read thread job cancelled!");
            }
        }
    }
}
