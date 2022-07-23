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
    /// Interaction logic for SettingDialog.xaml
    /// </summary>
    public partial class SettingDialog : UserControl
    {
        public event EventHandler OnDialogClose;

        public class Name
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private List<Name> items = new List<Name>();

        public SettingDialog()
        {
            InitializeComponent();
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
        }

        public void AddItem(string type, string value)
        {
            items.Add(new Name { FirstName = type, LastName = value });
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose?.Invoke(this, null);
        }

        private void ListViewTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
