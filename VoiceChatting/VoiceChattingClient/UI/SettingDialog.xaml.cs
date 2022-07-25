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
            InitializeSettingValues();
        }

        public void InitializeSettingValues()
        {
            ComboBoxInputDevices.Items.Clear();
            var microphoneList = MicrophoneController.GetMicrophonesList();

            foreach (var microphoneName in microphoneList)
            {
                var microphoneTextBlock = new TextBlock();
                microphoneTextBlock.Text = microphoneName;
                ComboBoxInputDevices.Items.Add(microphoneTextBlock);
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose?.Invoke(this, null);
        }
    }
}
