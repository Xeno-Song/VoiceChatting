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

        public void InitializeInputDeviceComboBoxIndex()
        {
            ComboBoxInputDevices.Items.Clear();
            var microphoneList = MicrophoneController.GetMicrophonesList();

            foreach (var microphoneName in microphoneList)
            {
                var microphoneTextBlock = new TextBlock();
                microphoneTextBlock.Text = microphoneName;
                ComboBoxInputDevices.Items.Add(microphoneTextBlock);
            }

            string usedMicrophoneDeviceName = Common.Config["Audio"]["MicrophoneDeviceName"] as string;
            if (microphoneList.Contains(usedMicrophoneDeviceName) == false)
            {
                Common.Config["Audio"]["MicrophoneDeviceName"] = string.Empty;
                Common.Config["Audio"].Save();
            }
            else
            {
                var microphoneIndex = microphoneList.IndexOf(usedMicrophoneDeviceName);
                ComboBoxInputDevices.SelectedIndex = microphoneIndex;
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeInputDeviceComboBoxIndex();
        }
    }
}
