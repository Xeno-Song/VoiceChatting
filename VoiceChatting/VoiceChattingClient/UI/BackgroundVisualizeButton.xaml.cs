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
        public Brush NormalBackground
        {
            get => (Brush)GetValue(PropertyNormalBackground);
            set => SetValue(PropertyNormalBackground, value);
        }
        public Brush MouseOverBackground {
            get => (Brush)GetValue(PropertyMouseOverBackground);
            set => SetValue(PropertyMouseOverBackground, value);
        }
        public Brush MouseClickBackground
        {
            get => (Brush)GetValue(PropertyMouseClickBackground);
            set => SetValue(PropertyMouseClickBackground, value);
        }
        public double BackgroundRadius
        {
            get => (double)GetValue(PropertyBackgroundRadius);
            set => SetValue(PropertyBackgroundRadius, value);
        }
        public Thickness ButtonMargin
        {
            get => (Thickness)GetValue(PropertyButtonMargin);
            set => SetValue(PropertyButtonMargin, value);
        }
        

        public static readonly DependencyProperty PropertyButtonForeground =
            DependencyProperty.Register(
                "ButtonForeground", typeof(Brush),
                typeof(BackgroundVisualizeButton),
                new PropertyMetadata(null));
        public static readonly DependencyProperty PropertyNormalBackground =
            DependencyProperty.Register(
                "NormalBackground", typeof(Brush),
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
        public static readonly DependencyProperty PropertyButtonMargin =
             DependencyProperty.Register(
                "ButtonMargin", typeof(Thickness),
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
            backgroundRectangle.Fill = NormalBackground;
        }

    }
}
