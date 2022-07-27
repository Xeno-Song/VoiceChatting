using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VoiceChattingClient.CommonObjects;
using VoiceChattingClient.CommonObjects.Config;
using VoiceChattingClient.Configs;
using VoiceChattingClient.SoundSystem;

namespace VoiceChattingClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MicrophoneController microphoneController;
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
            microphoneController.OnDataAvaliable += MicrophoneController_OnDataReceived;
        }

        private void MicrophoneController_OnDataReceived(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() => progressBarInputLevel.Value = microphoneController.InputLevel);
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
            Common.Log["main"].Info("Headset Button Clicked");
            MessageBox.Show("Headset Button Clicked");
        }

        private void OnButtonMicrophoneClick(object sender, RoutedEventArgs e)
        {
            var isDeviceOpened = microphoneController.OpenDevice(Common.Config["Audio"]?["MicrophoneDeviceName"] as string);
            if (!isDeviceOpened) MessageBox.Show("Inavlid device name!");
        }

        private void buttonHostServer_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Button Clicked");
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Common.Log.Dispose();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            microphoneController.CloseDevice();
        }
    }
}
