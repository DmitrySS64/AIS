using Microsoft.SqlServer.Server;
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
using System.Windows.Shapes;

namespace AIS_Client.View
{
    /// <summary>
    /// Логика взаимодействия для ConfirmDeleteWindow.xaml
    /// </summary>
    public partial class ConfirmDeleteWindow : Window
    {
        public ConfirmDeleteWindow(string content)
        {
            InitializeComponent();
            ContentTextBox.Text = content;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
