using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Vkontakte.Pages;
using static Vkontakte.Methods.Account;

namespace Vkontakte
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CheckAuthorization();
        }

        private void CheckAuthorization()
        {
            // Здесь проверяем, существует ли токен авторизации
            bool isAuthorized = CheckToken();

            if (isAuthorized)
            {
                // Если пользователь авторизован, переходим на HomePage
                MainFrame.Navigate(new HomePage());
            }
            else
            {
                // Если пользователь не авторизован, переходим на LoginPage
                MainFrame.Navigate(new LoginPage());
            }
        }

        private bool CheckToken()
        {
            // Здесь можно сделать проверку наличия токена в настройках или кэше
            // Возвращаем true, если токен существует и действителен
            // Возвращаем false, если токен отсутствует или недействителен
            return !string.IsNullOrEmpty(Properties.Settings.Default.AccessToken);
        }
    }
}


//POST https://api.vk.com/method/status.get?user_id=743784474&v=5.131 HTTP/1.1
//Authorization: Bearer vk1.a.8mo8TRf0jm67Nla3W3fFbe9qKhMNNMqg21DQLgaPUj...

//    https://api.vk.com/method/status.get?user_id=743784474&v=5.131&access_token=vk1.a.8mo8TRf0jm67Nla3W3fFbe9qKhMNNMqg21DQLgaPUj...