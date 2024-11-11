using AIS_Client.Model;
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
    /// Логика взаимодействия для CreateModalWindow.xaml
    /// </summary>
    public partial class CreateModalWindow : Window
    {
        public Dictionary<string,object> FormData {  get; private set; }
        public CreateModalWindow(FormDescription formDescription)
        {
            InitializeComponent();
            DynamicForm.BuildForm(formDescription);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            FormData = DynamicForm.FormData;
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
