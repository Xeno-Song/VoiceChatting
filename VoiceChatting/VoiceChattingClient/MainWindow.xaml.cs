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

namespace VoiceChattingClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsWindowMoving { get; private set; } = false;
        private Point? windowMoveLastPoint = null;

        public MainWindow()
        {
            InitializeComponent();
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
    }
}
