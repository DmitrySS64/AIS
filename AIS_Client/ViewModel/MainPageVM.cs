using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Windows;
using System.Collections.ObjectModel;
using AIS_Client.Model;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using AIS_Client.View.Pages;
using System.Windows.Navigation;
using System.Windows.Controls;
using AIS_Client.Utilities;

namespace AIS_Client.ViewModel
{
    public class MainPageVM : ViewModelBase
    {
        private UdpClient _client;
        private IPEndPoint _serverEndPoint;
        private int _indexCurrentClass;
        private string _serverResponse;
        private ObservableCollection<string> _classes;
        private string _currentClass;
        private bool _visibleAddBtn;
        private bool _visibleEditBtn;
        private bool _visibleDeleteBtn;
        private ObservableCollection<Dictionary<string, object>> _entities;

        //private INavigationService _navigationService;

        public ObservableCollection<string> Classes
        {
            get => _classes;
            set { _classes = value; OnPropertyChanged(nameof(Classes)); }
        }
        public string ServerResponse
        {
            get => _serverResponse;
            set { _serverResponse = value; OnPropertyChanged(nameof(ServerResponse)); }
        }
        public string CurrentClass
        {
            get => _currentClass;
            set 
            { 
                _currentClass = value;
                ListEntitiesCommand.Execute(null);
                OnPropertyChanged(nameof(CurrentClass)); 
            }
        }
        public ObservableCollection<Dictionary<string, object>> Entities
        {
            get => _entities;
            set { _entities = value; OnPropertyChanged(nameof(Entities)); }
        }
        public bool VisibleAddBtn
        {
            get => _visibleAddBtn;
            set { _visibleAddBtn = value; OnPropertyChanged(nameof(VisibleAddBtn)); } 
        }
        public bool VisibleEditBtn 
        { 
            get => _visibleEditBtn; 
            set { _visibleEditBtn = value; OnPropertyChanged(nameof(VisibleEditBtn)); }
        }
        public bool VisibleDeleteBtn 
        { 
            get => _visibleDeleteBtn;
            set { _visibleDeleteBtn = value; OnPropertyChanged(nameof(VisibleDeleteBtn)); }
        }

        public ICommand LoadClassesCommand { get; }
        public ICommand ListEntitiesCommand { get; }
        public ICommand AddEntityCommand { get; }
        public ICommand EditEntityCommand { get; }
        public ICommand DeleteEntityCommand { get; }
        

        public MainPageVM()
        {
            _client = new UdpClient();
            _serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
            _indexCurrentClass = -1;

            Classes = new ObservableCollection<string>();
            Entities = new ObservableCollection<Dictionary<string, object>>();

            LoadClassesCommand = new RelayCommand(LoadClasses);
            ListEntitiesCommand = new RelayCommand(ListEntities);
            AddEntityCommand = new RelayCommand(AddEntity);
            EditEntityCommand = new RelayCommand(EditEntity);
            DeleteEntityCommand = new RelayCommand(DeleteEntity);

            LoadClassesCommand.Execute(null);
        }

        private void LoadClasses()
        {
            SendRequest(new Request { Command = Commands.getClasses });
            var response = ReceiveResponse();
            if (response == null) return;

            Classes = new ObservableCollection<string>(JsonConvert.DeserializeObject<List<string>>(response));
            _indexCurrentClass = Classes.Count > 0 ? 0 : -1;
            CurrentClass = _indexCurrentClass != -1 ? Classes[_indexCurrentClass] : "none";
        }

        #region CRUD
        private void ListEntities()
        {
            if (_indexCurrentClass == -1)
            {
                ServerResponse = "No entity selected.";
                return;
            }

            SendRequest(new Request { ClassName = CurrentClass, Command = Commands.get });
            var response = ReceiveResponse();
            if (response == null) return;

            Entities = new ObservableCollection<Dictionary<string, object>>(
                JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response));
        }

        private void AddEntity()
        {
        }
        private void DeleteEntity()
        {
            throw new NotImplementedException();
        }

        private void EditEntity()
        {
            throw new NotImplementedException();
        }
        #endregion CRUD

        private void SendRequest(Request request)
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
                _client.Send(data, data.Length, _serverEndPoint);
            }
            catch (Exception ex) {
                ServerResponse += ex.Message;
                LaunchErrorPage(ex.Message);
            }
        }

        private string ReceiveResponse()
        {
            try
            {
                var receivedData = _client.Receive(ref _serverEndPoint);
                return Encoding.UTF8.GetString(receivedData);
            }
            catch (Exception ex) {
                ServerResponse += ex.Message;
                LaunchErrorPage(ex.Message);
                return null;
            }
            
        }

        private void LaunchErrorPage(string error = null)
        {
            //_navigationService.NavigateTo(typeof(ErrorPageVM));
            Application.Current.Dispatcher.Invoke(() =>
            {
                var errorPage = new ErrorPage();
                var frame = Application.Current.MainWindow.FindName("MainFrame") as Frame;
                frame?.Navigate(errorPage);
            });
            
        }
    }
}
