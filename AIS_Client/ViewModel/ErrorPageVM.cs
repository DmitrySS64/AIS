using AIS_Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace AIS_Client.ViewModel
{
    public class ErrorPageVM: ViewModelBase
    {
        //private readonly INavigationService _navigationService;
        //public ICommand ReloadPageCommand { get; }
        //public ErrorPageVM(INavigationService navigationService)
        //{
        //    _navigationService = navigationService;

        //    ReloadPageCommand = new RelayCommand((object obj) => _navigationService.GoBack());
        //}
        //private NavigationService _navigationService;
        public ErrorPageVM() { 
            //_navigationService = navigationService;
            //ReloadPageCommand => new RelayCommand(GoBack);
        }

        public ICommand ReloadPageCommand;
    }
}
