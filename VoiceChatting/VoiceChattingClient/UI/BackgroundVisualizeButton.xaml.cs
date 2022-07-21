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
            get => (Brush)GetValue(ButtonForegroundProperty);
            set => SetValue(ButtonForegroundProperty, value);
        }
        public Brush MouseOverBackground
        {
            get => (Brush)GetValue(MouseOverBackgroundProperty);
            set => SetValue(MouseOverBackgroundProperty, value);
        }
        public Brush MouseClickBackground
        {
            get => (Brush)GetValue(MouseClickBackgroundProperty);
            set => SetValue(MouseClickBackgroundProperty, value);
        }
        public Brush ButtonBackground
        {
            get => (Brush)GetValue(ButtonBackgroundProperty);
            set => SetValue(ButtonBackgroundProperty, value);
        }
        public Brush BackgroundBorderBrush
        {
            get => (Brush)GetValue(BackgroundBorderBrushProperty);
            set => SetValue(BackgroundBorderBrushProperty, value);
        }
        public Thickness BackgroundBorderThickness
        {
            get => (Thickness)GetValue(BackgroundBorderThicknessProperty);
            set => SetValue(BackgroundBorderThicknessProperty, value);
        }
        public double BackgroundRadius
        {
            get => (double)GetValue(BackgroundRadiusProperty);
            set => SetValue(BackgroundRadiusProperty, value);
        }
        public double BorderRadius
        {
            get => (double)GetValue(BorderRadiusProperty);
            set => SetValue(BorderRadiusProperty, value);
        }
        public Thickness BorderMargin
        {
            get => (Thickness)GetValue(BorderMarginProperty);
            set => SetValue(BorderMarginProperty, value);
        }
        public Thickness ButtonPadding
        {
            get => (Thickness)GetValue(ButtonPaddingProperty);
            set => SetValue(ButtonPaddingProperty, value);
        }
        public object ButtonContent
        {
            get => (object)GetValue(ButtonContentProperty);
            set => SetValue(ButtonContentProperty, value);
        }
        public event RoutedEventHandler Click;


        public static readonly DependencyProperty ButtonForegroundProperty =
            DependencyProperty.Register(
                "ButtonForeground", typeof(Brush),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register(
                "MouseOverBackground", typeof(Brush),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty MouseClickBackgroundProperty =
            DependencyProperty.Register(
                "MouseClickBackground", typeof(Brush),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty BackgroundBorderBrushProperty =
            DependencyProperty.Register(
                "BackgroundBorderBrush", typeof(Brush),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty BackgroundRadiusProperty =
             DependencyProperty.Register(
                "BackgroundRadius", typeof(double),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty BorderRadiusProperty =
             DependencyProperty.Register(
                "BorderRadius", typeof(double),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty ButtonPaddingProperty =
             DependencyProperty.Register(
                "ButtonPadding", typeof(Thickness),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty BorderMarginProperty =
             DependencyProperty.Register(
                "BorderMargin", typeof(Thickness),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty BackgroundBorderThicknessProperty =
             DependencyProperty.Register(
                "BackgroundBorderThickness", typeof(Thickness),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty ButtonContentProperty =
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

        private void OnButtonClick(object sender, RoutedEventArgs e) => Click?.Invoke(this, e);
    }
}
