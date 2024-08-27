using AIS_Client.ViewModel;
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

namespace AIS_Client.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ErrorWindow.xaml
    /// </summary>
    public partial class ErrorPage : Page
    {

        public ErrorPage()
        {
            InitializeComponent();
            TextBlock.Text = "Ошибка отправки формы";
            //DataContext = new ErrorPageVM(()=>NavigationService.GoBack());
        }
    }
}
