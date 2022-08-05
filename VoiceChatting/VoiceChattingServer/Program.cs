using System;
using System.Net;
using System.Threading;
using VoiceChattingServer.Connection;

namespace VoiceChattingServer
{
    internal class Program
    {
        static VoiceServer server;

        static void Main(string[] args)
        {
            server = new VoiceServer("localhost", 11523);
            server.Bind();
            server.OnVoiceDataReceived += Server_OnVoiceDataReceived;

            while (Console.ReadKey().Key != ConsoleKey.Q)
            {
                Thread.Sleep(1);
            }
            server.Disconnect();
        }

        private static void Server_OnVoiceDataReceived(object sender, CommonObjects.DataModels.VoiceData.Model.VoiceData e)
        {
            Console.WriteLine(String.Format("Data Received! : [ Length : {0} ]", e.Header.Length));
            IPEndPoint dataSender = sender as IPEndPoint;

            server.SendVoiceData(dataSender, e.Data, e.Header.Length);
        }
    }
}
