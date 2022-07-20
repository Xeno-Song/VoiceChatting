﻿using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VoiceChattingClient.CommonObjects;

namespace VoiceChattingClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsWindowMoving { get; private set; } = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeLogFiles();
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
            Common.Log["main"].Info("Setting Button Clicked");
            MessageBox.Show("Setting Button Clicked");
        }

        private void OnButtonHeadsetClick(object sender, RoutedEventArgs e)
        {
            Common.Log["main"].Info("Headset Button Clicked");
            MessageBox.Show("Headset Button Clicked");
        }

        private void OnButtonMicrophoneClick(object sender, RoutedEventArgs e)
        {
            Common.Log["main"].Info("Microphone Button Clicked");
            MessageBox.Show("Microphone Button Clicked");
        }

        private void buttonHeadset_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void TextBoxIpAddress_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = (sender as TextBox).Text;

            if (!Regex.IsMatch(text, "^([0-9a-fA-F]{4}:){7}[0-9a-fA-F]{4}$"))
            {
                e.Handled = true;
            }
        }

        private void buttonHostServer_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Button Clicked");
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Common.Log.Dispose();
        }
    }
}
