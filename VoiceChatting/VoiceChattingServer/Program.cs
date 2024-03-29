﻿using CommonObjects.DataModels.VoiceData.Model;
using System;
using System.Collections.Generic;
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
            server = new VoiceServer();
            server.Bind(11523);
            server.OnVoiceDataReceived += Server_OnVoiceDataReceived;

            while (Console.ReadKey().Key != ConsoleKey.Q)
            {
                Thread.Sleep(1);
            }
            server.Close();
        }

        private static void Server_OnVoiceDataReceived(object sender, CommonObjects.DataModels.VoiceData.VoiceData e)
        {
            Console.WriteLine(String.Format("Data Received! : [ Length : {0} ]", e.Header.Length));
            if (e.Header.Command == 1)
            {
                var endPointList = new List<IPEndPoint>();
                endPointList.Add(new IPEndPoint(IPAddress.Loopback, 11523));
                server.SendHostList(e.Sender, endPointList);
            }

            IPEndPoint dataSender = sender as IPEndPoint;

            server.SendVoiceData(dataSender, e.WaveData.WaveData, e.Header.Length);
        }
    }
}
