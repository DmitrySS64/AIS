using AIS_Client.View.Pages;
using AIS_Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace AIS_Client
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);

        //    var mainWindow = new MainWindow();
        //    var navigationService = new Utilities.NavigationService(mainWindow.MainFrame);

        //    navigationService.Configure<MainPageVM, MainPage>();
        //    navigationService.Configure<ErrorPageVM, ErrorPage>();

        //    var mainViewModel = new MainVM(navigationService);
        //    mainWindow.DataContext = mainViewModel;

        //    mainWindow.Show();
        //}
    }
}
