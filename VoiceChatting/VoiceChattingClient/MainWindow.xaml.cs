﻿using NAudio.Wave;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VoiceChattingClient.Codec;
using CommonObjects;
using CommonObjects.Config;
using VoiceChattingClient.Configs;
using VoiceChattingClient.Connection;
using VoiceChattingClient.SoundSystem;
using CommonObjects.DataModels.VoiceData.Model;
using System.Net;
using CommonObjects.DataModels.VoiceData;

namespace VoiceChattingClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MicrophoneController microphoneController;
        private SpeakerController speakerController;
        private VoiceClient voiceClient;
        private AsioOut asioOut;
        private OpusCodec codec;

        public MainWindow()
        {
            InitializeComponent();
            InitializeLogFiles();
            InitializeConfigDatas();

            InitializeControlObjects();
        }

        private void InitializeLogFiles()
        {
            string logBaseDirectory = "../log/";
            if (Directory.Exists(logBaseDirectory) == false)
            {
                var directory = Directory.CreateDirectory(logBaseDirectory);
                if (directory.Exists == false)
                {
                    MessageBox.Show("Error while initialize log files.");
                    throw new DirectoryNotFoundException(
                        "Cannot make log directroy");
                }
            }

            Common.Log.CreateLogger("main", logBaseDirectory + "main.log");
            SettingControl.OnDialogClose += SettingControl_OnDialogClose;
        }

        private void InitializeConfigDatas()
        {
            string configBaseDirectory = "../config/";
            string configFileName;
            IConfig config;

            // Config save directory exist check
            if (!Directory.Exists(configBaseDirectory))
            {
                var directory = Directory.CreateDirectory(configBaseDirectory);
                if (directory.Exists == false)
                {
                    MessageBox.Show("Cannot make directory to save config in " + configBaseDirectory);
                    return;
                }
            }

            configFileName = "Audio.json";
            config = new AudioConfig()
            {
                FilePath = configBaseDirectory + configFileName,
                CreateIfNotExist = true
            };
            if (config.Load() == false) MessageBox.Show("Failed to save audio config");
            Common.Config["Audio"] = config;
        }

        private void InitializeControlObjects()
        {
            microphoneController = new MicrophoneController();
            speakerController = new SpeakerController();
            codec = new OpusCodec();

            var isDeviceOpened = speakerController.OpenDevice(Common.Config["Audio"]?["SpeakerDeviceName"] as string);
            if (!isDeviceOpened) MessageBox.Show("Inavlid device name!");

            voiceClient = new VoiceClient(2048);
            // microphoneController.OnDataAvaliable += MicrophoneController_OnDataReceived;
            microphoneController.OnDataAvaliable += (object sender, WaveInEventArgs e) =>
            {
                voiceClient.SendVoiceData(
                    new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11523),
                    e.Buffer, e.BytesRecorded);
            };
            voiceClient.OnVoiceDataReceived += (object sender, VoiceData data) =>
            {
                if (data.Header.Command == 0)
                {
                    speakerController.AddPlaybackBytes(data.WaveData.WaveData, data.Header.Length);
                }
            };
        }

        private void MicrophoneController_OnDataReceived(object sender, WaveInEventArgs e)
        {
            // Dispatcher.Invoke(() => progressBarInputLevel.Value = microphoneController.InputLevel);
            

            // Debug.WriteLine(String.Format(
            //     "Encoded Size : {0}, Changed : {1}, Ratio : {2:0.00} %",
            //     encodedSize,
            //     e.BytesRecorded - encodedSize,
            //     ((double)encodedSize / e.BytesRecorded) * 100));
            speakerController.AddPlaybackBytes(e.Buffer, e.BytesRecorded);
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnMaximizeButtonClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else WindowState = WindowState.Maximized;
        }

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void OnTopBarDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OnMaximizeButtonClick(sender, null);
        }

        private void OnMouseLeftBottonForMoveWindow(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            base.DragMove();
        }

        private void OnButtonSettingClick(object sender, RoutedEventArgs e)
        {
            popupBackgroundMask.Visibility = Visibility.Visible;
            SettingControl.Visibility = Visibility.Visible;
        }

        private void SettingControl_OnDialogClose(object sender, System.EventArgs e)
        {
            popupBackgroundMask.Visibility = Visibility.Collapsed;
            SettingControl.Visibility = Visibility.Collapsed;
        }

        private void OnButtonHeadsetClick(object sender, RoutedEventArgs e)
        {
            var deviceList = AsioOut.GetDriverNames();
            asioOut = new AsioOut(Common.Config["Audio"]?["MicrophoneDeviceName"] as string);
            var inputChannels = asioOut.DriverInputChannelCount;

            asioOut.InputChannelOffset = 4;
            var recordChannelCount = 2;
            var sampleRate = 44100;
            asioOut.InitRecordAndPlayback(null, recordChannelCount, sampleRate);
            asioOut.AudioAvailable += Asio_OnDataReceived;
        }

        private void Asio_OnDataReceived(object sender, AsioAudioAvailableEventArgs e)
        {
            // var sample = e.GetAsInterleavedSamples();
            // speakerController.AddPlaybackBytes()
        }

        private void OnButtonMicrophoneClick(object sender, RoutedEventArgs e)
        {
            var isDeviceOpened = microphoneController.OpenDevice(Common.Config["Audio"]?["MicrophoneDeviceName"] as string);
            if (!isDeviceOpened) MessageBox.Show("Inavlid device name!");
        }

        private void buttonHostServer_Click(object sender, RoutedEventArgs e)
        {
            voiceClient.Bind(11523);
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Common.Log.Dispose();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            microphoneController.CloseDevice();
        }

        private void buttonConnectToServer_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
