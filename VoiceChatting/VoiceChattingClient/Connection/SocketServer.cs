using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VoiceChattingClient.Connection
{
    internal class TcpServer
    {
        private Socket serverSocket = null;
        public string BindIP { get; set; }
        private Thread socketListenThread = null;

        internal TcpServer()
        {
        }

        internal bool Connect(string bindHost, int port)
        {
            var hostEntry = Dns.GetHostEntry(BindIP);
            if (hostEntry.AddressList.Count() == 0) return false;
            IPAddress address = hostEntry.AddressList[0];
            var localEndPoint = new IPEndPoint(address, port);

            Socket listener = new Socket(address.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);
            }
            catch (Exception e)
            {
                
            }


                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        private void SocketListenJob()
        {

        }
    }
}
