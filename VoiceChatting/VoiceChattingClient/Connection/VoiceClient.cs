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
using CommonObjects.DataModels.VoiceData;
using CommonObjects;

namespace VoiceChattingClient.Connection
{
    internal class VoiceClient : IDisposable
    {
        /// <summary>
        /// Binded port number
        /// </summary>
        public int Port { get; private set; }

        public event EventHandler<VoiceData> OnVoiceDataReceived;

        // Socket send/receive objects
        private UdpClient socket = null;
        private byte[] DataSendBuffer { get; }

        // Data receive thread objcets
        private Thread socketReceiveThread;
        private bool isSocketClosing = false;
        private CancellationTokenSource dataReceiveCancellationTokenSource;


        public VoiceClient(int bufferSize)
        {
            DataSendBuffer = Common.BufferManager.TakeBuffer(bufferSize);
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
        /// Send client connect message to voice chatting manage server
        /// </summary>
        /// <param name="serverAddress">Management server address</param>
        /// <param name="port">Server port</param>
        /// <returns>If success to stop, return <see langword="true"/>, otherwise <see langword="false"/></returns>
        public bool ConnectToServer(string serverAddress, int port)
        {
            if (socket == null) return false;

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverAddress), port);
            var data = new VoiceData();// new SocketVoiceDataParser(dataCodingBufferPool);
            data.Header.Command = 1;
            data.Header.Length = 0;

            lock(socket)
            {
                var dataSize = data.ToBytes(DataSendBuffer, DataSendBuffer.Length);
                socket.SendAsync(DataSendBuffer, dataSize, endPoint).Wait();
            }

            data.Dispose();
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
        /// <param name="data">Voice data as byte array</param>
        /// <param name="voiceDataLen">Voice data len</param>
        public void SendVoiceData(IPEndPoint endPoint, byte[] data, int voiceDataLen)
        {
            var voiceData = new VoiceData();
            voiceData.Data = new VoiceWaveData(Common.BufferManager.TakeBuffer(voiceDataLen));
            voiceData.WaveData.CopyFrom(data, 0, voiceDataLen);

            lock(socket)
            {
                byte[] sendBuffer = DataSendBuffer;
                int dataSize = voiceData.ToBytes(DataSendBuffer, DataSendBuffer.Length);
                socket.SendAsync(sendBuffer, dataSize).Wait();
            }

            voiceData.Dispose();
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

                        var voiceData = VoiceData.Parse(buffer, 0, buffer.Length);
                        voiceData.Sender = endPoint;
                        
                        OnVoiceDataReceived?.Invoke(this, voiceData);
                        voiceData.Dispose();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Read thread job cancelled!");
            }
        }

        public void Dispose()
        {
            Common.BufferManager.ReturnBuffer(DataSendBuffer);
        }
    }
}
