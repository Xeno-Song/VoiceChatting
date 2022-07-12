using System.Windows;
using System.Windows.Controls;

namespace VoiceChattingClient.Overlays
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public double BackgroundRound
        {
            get => (double)GetValue(PropertyBackgroundRound);
            set => SetValue(PropertyBackgroundRound, value);
        }

        public static readonly DependencyProperty PropertyBackgroundRound =
           DependencyProperty.Register(
               "BackgroundRound", typeof(double),
               typeof(Settings), new PropertyMetadata(5.0));

        public Settings()
        {
            InitializeComponent();
        }
    }
}
