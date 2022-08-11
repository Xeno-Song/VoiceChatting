using CommonObjects.DataModels.VoiceData;
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
        /// <summary>
        /// Binded port number
        /// </summary>
        public int Port { get; private set; }

        public event EventHandler<VoiceData> OnVoiceDataReceived;

        // Socket send/receive objects
        private UdpClient socket = null;
        public ByteMemoryPool dataCodingBufferPool;
        private int dataSendBufferMemoryPoolIndex;
        private byte[] DataSendBuffer { get => dataCodingBufferPool[dataSendBufferMemoryPoolIndex]; }

        // Data receive thread objcets
        private Thread socketReceiveThread;
        private bool isSocketClosing = false;
        private CancellationTokenSource dataReceiveCancellationTokenSource;


        public VoiceServer(int bufferSize)
        {
            dataCodingBufferPool = new ByteMemoryPool(bufferSize, 10);
            dataSendBufferMemoryPoolIndex = dataCodingBufferPool.LockBuffer();
        }

        /// <summary>
        /// Open udp socket
        /// </summary>
        /// <param name="port">Bind target port</param>
        /// <returns>If success to open socket, return <see langword="true"/>,
        /// but if socket was already opened or failed to create socket, return <see langword="false"/></returns>
        public bool Bind(int port)
        {
            Port = port;
            if (socket != null) return false;

            socket = new UdpClient(Port);
            socketReceiveThread = new Thread(VoiceDataReceiveThread);
            socketReceiveThread.Start();

            return true;
        }

        /// <summary>
        /// Close opened socket
        /// </summary>
        public void Close()
        {
            // Stop running data receive thread
            isSocketClosing = true;
            StopVoiceDataReceiveThread();

            // Disconnect socket and dispose
            socket.Close();
            socket.Dispose();
            socket = null;
        }

        /// <summary>
        /// Send voice data to endpoint
        /// </summary>
        /// <param name="endPoint">Target point for data send</param>
        /// <param name="voiceData">Voice data as byte array</param>
        /// <param name="voiceDataLen">Voice data len</param>
        public void SendVoiceData(IPEndPoint endPoint, byte[] voiceData, int voiceDataLen)
        {
            var voiceDataParser = new SocketVoiceDataParser(dataCodingBufferPool);
            voiceDataParser.CopyVoiceData(voiceData, voiceDataLen);

            lock (socket)
            {
                byte[] sendBuffer = DataSendBuffer;
                int dataSize = voiceDataParser.ToBytes(buffer: ref sendBuffer);
                socket.SendAsync(sendBuffer, dataSize).Wait();
            }

            voiceDataParser.Dispose();
        }

        public void SendHostList(IPEndPoint endPoint, List<IPEndPoint> endPointList)
        {
            var voiceDataParser = new SocketVoiceDataParser(dataCodingBufferPool);
            voiceDataParser.EndPointList = endPointList;

            lock (socket)
            {
                byte[] sendBuffer = DataSendBuffer;
                int dataSize = voiceDataParser.ToBytes(buffer: ref sendBuffer);
                socket.SendAsync(sendBuffer, dataSize).Wait();
            }

            voiceDataParser.Dispose();
        }

        /// <summary>
        /// Stop data receive thread
        /// </summary>
        /// <param name="timeout">Thread join time when cancellation token active</param>
        /// <returns>If success to stop, return <see langword="true"/>, otherwise <see langword="false"/></returns>
        /// <exception cref="Exception"></exception>
        private bool StopVoiceDataReceiveThread(int timeout = 10)
        {
            if (socketReceiveThread != null && socketReceiveThread.IsAlive)
            {
                // Thread is running but cancellation token was not define
                // In this case, cannot abort thread job
                if (dataReceiveCancellationTokenSource == null)
                    throw new Exception("Thread is running but cancellation token was not defined!");

                dataReceiveCancellationTokenSource.Cancel();
                socketReceiveThread.Join(timeout);

                dataReceiveCancellationTokenSource = null;
                socketReceiveThread = null;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Voice data receive thread job
        /// </summary>
        private void VoiceDataReceiveThread()
        {
            // Reset cancellation token to abort receive job
            dataReceiveCancellationTokenSource = new CancellationTokenSource();
            var dataReceiveCancellationToken = dataReceiveCancellationTokenSource.Token;

            try
            {
                // Register data receive statement to cancellation exception hanlder
                using (dataReceiveCancellationToken.Register(Thread.CurrentThread.Abort))
                {
                    while (!isSocketClosing)
                    {
                        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, Port);
                        var buffer = socket.Receive(ref endPoint);

                        var data = VoiceData.Parse(buffer, 0, buffer.Length);
                        data.Sender = endPoint;

                        OnVoiceDataReceived?.Invoke(this, data);
                        data.Dispose();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Read thread job cancelled!");
            }
        }
    }
}
