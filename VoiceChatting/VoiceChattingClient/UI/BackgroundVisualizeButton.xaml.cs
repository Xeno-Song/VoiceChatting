using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VoiceChattingClient.UI
{
    /// <summary>
    /// Interaction logic for BackgroundVisualizeButton.xaml
    /// </summary>
    public partial class BackgroundVisualizeButton : UserControl
    {
        public Brush ButtonForeground
        {
            get => (Brush)GetValue(PropertyButtonForeground);
            set => SetValue(PropertyButtonForeground, value);
        }
        public Brush MouseOverBackground
        {
            get => (Brush)GetValue(PropertyMouseOverBackground);
            set => SetValue(PropertyMouseOverBackground, value);
        }
        public Brush MouseClickBackground
        {
            get => (Brush)GetValue(PropertyMouseClickBackground);
            set => SetValue(PropertyMouseClickBackground, value);
        }
        public Brush ButtonBackground
        {
            get => (Brush)GetValue(ButtonBackgroundProperty);
            set => SetValue(ButtonBackgroundProperty, value);
        }
        public double BackgroundRadius
        {
            get => (double)GetValue(PropertyBackgroundRadius);
            set => SetValue(PropertyBackgroundRadius, value);
        }
        public Thickness ButtonPadding
        {
            get => (Thickness)GetValue(PropertyButtonPadding);
            set => SetValue(PropertyButtonPadding, value);
        }
        public object ButtonContent
        {
            get => (object)GetValue(PropertyButtonContent);
            set => SetValue(PropertyButtonContent, value);
        }
        public event RoutedEventHandler Click;


        public static readonly DependencyProperty PropertyButtonForeground =
            DependencyProperty.Register(
                "ButtonForeground", typeof(Brush),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty PropertyMouseOverBackground =
            DependencyProperty.Register(
                "MouseOverBackground", typeof(Brush),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty PropertyMouseClickBackground =
            DependencyProperty.Register(
                "MouseClickBackground", typeof(Brush),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty PropertyBackgroundRadius =
             DependencyProperty.Register(
                "BackgroundRadius", typeof(double),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty PropertyButtonPadding =
             DependencyProperty.Register(
                "ButtonPadding", typeof(Thickness),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty PropertyButtonContent =
             DependencyProperty.Register(
                "ButtonContent", typeof(object),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty ButtonBackgroundProperty =
            DependencyProperty.Register(
                "ButtonBackground", typeof(Brush),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));

        public BackgroundVisualizeButton()
        {
            InitializeComponent();
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            backgroundRectangle.Fill = MouseOverBackground;
        }
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            backgroundRectangle.Fill = ButtonBackground;
        }

        private void OnButtonClick(object sender, RoutedEventArgs e) => Click(this, e);
    }
}
