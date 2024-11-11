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
using System.Data;
using AIS_Client.View;
using System.Security.Permissions;
using System.Windows.Markup;

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
        private Visibility _visibleAddBtn;
        private Visibility _visibleEditBtn = Visibility.Hidden;
        private Visibility _visibleDeleteBtn = Visibility.Hidden;
        private DataTable _entities;
        private DataRowView _selectedEntity;

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
        public DataTable Entities
        {
            get => _entities;
            set { _entities = value; OnPropertyChanged(nameof(Entities)); }
        }
        public DataRowView SelectedEntity
        {
            get => _selectedEntity;
            set
            {
                _selectedEntity = value;
                if (_selectedEntity != null) {
                    VisibleEditBtn = Visibility.Visible;
                    VisibleDeleteBtn = Visibility.Visible;
                }
                else {
                    VisibleEditBtn = Visibility.Hidden;
                    VisibleDeleteBtn = Visibility.Hidden;
                }
                OnPropertyChanged(nameof(SelectedEntity));
            }
        }
        public Visibility VisibleAddBtn
        {
            get => _visibleAddBtn;
            set { _visibleAddBtn = value; OnPropertyChanged(nameof(VisibleAddBtn)); } 
        }
        public Visibility VisibleEditBtn 
        { 
            get => _visibleEditBtn; 
            set { _visibleEditBtn = value; OnPropertyChanged(nameof(VisibleEditBtn)); }
        }
        public Visibility VisibleDeleteBtn 
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
            Entities = new DataTable();

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

            var dictionaries = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response);
            Entities = ConvertToDataTable(dictionaries);
        }

        private void AddEntity() 
        {
            SendRequest(new Request { ClassName = CurrentClass, Command = Commands.getForm });
            var formResponse = ReceiveResponse();
            var form = JsonConvert.DeserializeObject<FormDescription>(formResponse);

            var addEntityWindow = new CreateModalWindow(form);
            if (addEntityWindow.ShowDialog() == true)
            {
                // Получаем данные, введенные пользователем
                var data = addEntityWindow.FormData;

                // Отправляем запрос на добавление новой записи на сервер
                var request = new Request
                {
                    ClassName = CurrentClass,
                    Command = Commands.add,
                    Payload = JsonConvert.SerializeObject(data)
                };
                SendRequest(request);
                var response = ReceiveResponse();
                Console.WriteLine($"Ответ сервера: {response}");

                ListEntities();
            }
            else
            {
                Console.WriteLine("Добавление записи отменено.");
            }
        }
        private void DeleteEntity()
        {
            int id;
            if (_selectedEntity != null && SelectedEntity["Id"] != DBNull.Value)
            {
                id = Convert.ToInt32(SelectedEntity["Id"]);
            }
            else
            {
                Console.WriteLine("No entity selected.");
                return;
            }


            var deleteEntityWindow = new ConfirmDeleteWindow($"id: { id }");
            if (deleteEntityWindow.ShowDialog() == true)
            {
                SendRequest(new Request { ClassName = CurrentClass, Command = Commands.delete, Id = id });
                var response = ReceiveResponse();
                Console.WriteLine("Response received: " + response);
                ListEntities();
            }
            else
            {
                Console.WriteLine("Удаление записи отменено.");
            }
        }

        private void EditEntity()
        {
            int id;
            if (_selectedEntity != null && SelectedEntity["Id"] != DBNull.Value)
            {
                id = Convert.ToInt32(SelectedEntity["Id"]);
            }
            else
            {
                Console.WriteLine("No entity selected.");
                return;
            }

            SendRequest(new Request { ClassName = CurrentClass, Command = Commands.getForm });
            var formResponse = ReceiveResponse();
            var form = JsonConvert.DeserializeObject<FormDescription>(formResponse);

            var editEntityWindow = new EditEntityWindow(_selectedEntity, form);

            //var editEntityWindow = new EditEntityWindow(_selectedEntity.);
            if (editEntityWindow.ShowDialog() == true)
            {
                var editedData = editEntityWindow.EditedData;

                var data = new Dictionary<string, object>();

                foreach (var field in editedData)
                {
                    data[field.Key] = field.Value;
                }

                var request = new Request
                {
                    ClassName = CurrentClass,
                    Command = Commands.update,
                    Id = id,
                    Payload = JsonConvert.SerializeObject(data)
                };
                SendRequest(request);
                var response = ReceiveResponse();
                Console.WriteLine("Response received: " + response);
                ListEntities();
            }
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

        private DataTable ConvertToDataTable(List<Dictionary<string, object>> dictionaries)
        {
            var dataTable = new DataTable();

            if (dictionaries == null || dictionaries.Count == 0)
                return dataTable;

            // Добавляем столбцы
            foreach (var key in dictionaries[0].Keys)
            {
                dataTable.Columns.Add(key);
            }

            // Добавляем строки
            foreach (var dict in dictionaries)
            {
                var row = dataTable.NewRow();
                foreach (var kvp in dict)
                {
                    row[kvp.Key] = kvp.Value ?? DBNull.Value;
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

    }
}
