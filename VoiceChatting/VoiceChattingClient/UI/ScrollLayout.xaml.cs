using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VoiceChattingClient.UI
{
    /// <summary>
    /// Interaction logic for ScrollLayout.xaml
    /// </summary>
    public partial class ScrollLayout : UserControl
    {
        public class Name
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private List<Name> items = new List<Name>();

        public Brush ItemMouseOverBackgroundColor
        {
            get => (Brush)GetValue(ItemMouseOverBackgroundProperty);
            set => SetValue(ItemMouseOverBackgroundProperty, value);
        }
        public Brush ItemSelectionBackgroundColor
        {
            get => (Brush)GetValue(ItemMouseOverBackgroundProperty);
            set => SetValue(ItemMouseOverBackgroundProperty, value);
        }
        public event RoutedEventHandler Click;


        public static readonly DependencyProperty ItemMouseOverBackgroundProperty =
            DependencyProperty.Register(
                "ItemMouseOverBackground", typeof(Brush),
                typeof(ScrollLayout),
                new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));
        public static readonly DependencyProperty ItemSelectionBackgroundColorProperty =
            DependencyProperty.Register(
                "ItemSelectionBackgroundColor", typeof(Brush),
                typeof(ScrollLayout),
                new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public ScrollLayout()
        {
            InitializeComponent();

            ListViewTest.ItemsSource = items;
            AddItem("test1", "test1");
            AddItem("test2", "test2");
            AddItem("test3", "test3");
            AddItem("test4", "test4");
            AddItem("test5", "test5");
            AddItem("test6", "test6");
            AddItem("test7", "test7");
            AddItem("test7", "test7");
            AddItem("test7", "test7");
            AddItem("test7", "test7");
            AddItem("test7", "test7");
            AddItem("test7", "test7");
            AddItem("test7", "test7");
            ListViewTest.Items.Refresh();
        }

        public void AddItem(string type, string value)
        {
            items.Add(new Name { FirstName = type, LastName = value });
        }


        private void ListViewTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewTest.SelectedItems.Count == 0) return;
            Name item = ListViewTest.SelectedItems[0] as Name;
            Debug.WriteLine("Item Selected - " + item.FirstName);
        }
    }
}
