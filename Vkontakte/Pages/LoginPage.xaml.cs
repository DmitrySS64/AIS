using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Vkontakte.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            Nav();
        }

        private void Nav()
        {
            string appId = Properties.Settings.Default.AppId;
            var uriStr = @"https://oauth.vk.com/authorize?client_id=" + appId + @"&scope=offline&redirect_uri=https://oauth.vk.com/blank.html&display=page&v=5.6&response_type=token";
            myWebBrowser.Navigate(new Uri(uriStr));
        }

        private void BrowserOnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Uri.AbsoluteUri.Contains("oauth.vk.com/blank.html"))
            {
                string url = e.Uri.Fragment;
                url = url.Trim('#');
                var accessToken = HttpUtility.ParseQueryString(url).Get("access_token");
                var userId = HttpUtility.ParseQueryString(url).Get("user_id");

                Properties.Settings.Default.AccessToken = accessToken;
                Properties.Settings.Default.UserId = userId;
                Properties.Settings.Default.Save();

                NavigationService.Navigate(new HomePage());
                //MessageBox.Show($"Access Token: {accessToken}\nUser ID: {userId}");
            }
        }
    }
}
