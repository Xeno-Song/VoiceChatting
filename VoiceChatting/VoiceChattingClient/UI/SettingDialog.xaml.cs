using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoiceChattingClient.CommonObjects;
using VoiceChattingClient.SoundSystem;

namespace VoiceChattingClient.UI
{
    /// <summary>
    /// Interaction logic for SettingDialog.xaml
    /// </summary>
    public partial class SettingDialog : UserControl
    {
        public event EventHandler OnDialogClose;

        public SettingDialog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeInputDeviceComboBoxIndex();
            InitializeOutputDeviceComboBoxIndex();
        }

        public void InitializeInputDeviceComboBoxIndex()
        {
            ComboBoxInputDevices.Items.Clear();
            var deviceList = MicrophoneController.GetMicrophonesList();

            foreach (var devicename in deviceList)
            {
                var textBlock = new TextBlock();
                textBlock.Text = devicename;
                ComboBoxInputDevices.Items.Add(textBlock);
            }

            string usedDeviceName = Common.Config["Audio"]["MicrophoneDeviceName"] as string;
            if (deviceList.Contains(usedDeviceName) == false)
            {
                Common.Config["Audio"]["MicrophoneDeviceName"] = string.Empty;
                Common.Config["Audio"].Save();
            }
            else
            {
                var deviceIndex = deviceList.IndexOf(usedDeviceName);
                ComboBoxInputDevices.SelectedIndex = deviceIndex;
            }
        }

        public void InitializeOutputDeviceComboBoxIndex()
        {
            ComboBoxOutputDevices.Items.Clear();
            var deviceList = SpeakerController.GetSpeakerList();

            foreach (var deviceName in deviceList)
            {
                var textBlock = new TextBlock();
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.Text = deviceName;
                ComboBoxOutputDevices.Items.Add(textBlock);
            }

            string usedDeviceName = Common.Config["Audio"]["SpeakerDeviceName"] as string;
            if (deviceList.Contains(usedDeviceName) == false)
            {
                Common.Config["Audio"]["SpeakerDeviceName"] = string.Empty;
                Common.Config["Audio"].Save();
            }
            else
            {
                var deviceIndex = deviceList.IndexOf(usedDeviceName);
                ComboBoxOutputDevices.SelectedIndex = deviceIndex;
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose?.Invoke(this, null);
        }

        private void ComboBoxInputDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxInputDevices.SelectedIndex == -1) return;
            TextBlock selectedTextBlock = ComboBoxInputDevices.SelectedItem as TextBlock;
            Common.Config["Audio"]["MicrophoneDeviceName"] = selectedTextBlock.Text;
            Common.Config["Audio"].Save();
        }

        private void ComboBoxOutputDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxOutputDevices.SelectedIndex == -1) return;
            TextBlock selectedTextBlock = ComboBoxOutputDevices.SelectedItem as TextBlock;
            Common.Config["Audio"]["SpeakerDeviceName"] = selectedTextBlock.Text;
            Common.Config["Audio"].Save();
        }
    }
}
