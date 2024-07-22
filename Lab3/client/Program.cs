using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Lab2_client.Model;
using Lab2_client.Model.Entity;
using Validator = Lab2_client.Model.Validator;

namespace Lab2_client
{
    public class UdpClientApp
    {
        private const int serverPort = 11000;
        private const string serverAddress = "127.0.0.1";
        private static List<string> menu = new List<string>() 
        { 
            "Get", 
            "GetById",
            "Add",
            "Update", 
            "Delete", 
            "Exit" 
        };

        static void Main()
        {
            UdpClient client = new UdpClient();
            client.Connect(serverAddress, serverPort);
            bool Work = true;
            while (Work)
            {
                //Console.WriteLine("Enter command (Get, GetById, Add, Update, Delete, Exit):");
                View.Menu(menu, "Enter command", false);
                string command = Console.ReadLine().ToUpper();
                string requestData = string.Empty;

                switch (command)
                {
                    case COMMANDS.GET:
                    case "0":
                        requestData = JsonSerializer.Serialize(new Request { Command = "Get" });
                        break;
                    case COMMANDS.GET_BY_ID:
                    case "1":
                        Console.WriteLine("Enter ID:");
                        int getId = int.Parse(Console.ReadLine());
                        requestData = JsonSerializer.Serialize(new Request { Command = "GetById", Id = getId });
                        break;
                    case COMMANDS.ADD:
                    case "2":
                        Student? newStudent = CreateNewRecord();
                        if (newStudent == null) continue;
                        requestData = JsonSerializer.Serialize(new Request { Command = "Add", Payload = JsonSerializer.Serialize(newStudent) });
                        break;
                    case COMMANDS.UPDATE:
                    case "3":
                        Console.WriteLine("Enter ID:");
                        int updateId = int.Parse(Console.ReadLine());
                        requestData = JsonSerializer.Serialize(new Request { Command = "GetById", Id = updateId });
                        Student student = JsonSerializer.Deserialize<Student>(GetTheServerResponse(client, requestData));
                        Student? updatedStudent = CreateNewRecord(student);
                        if (updatedStudent == null) continue;
                        requestData = JsonSerializer.Serialize(new Request { Command = "Update", Id = updateId, Payload = JsonSerializer.Serialize(updatedStudent) });
                        break;
                    case COMMANDS.DELETE:
                    case "4":
                        Console.WriteLine("Enter ID:");
                        int deleteId = int.Parse(Console.ReadLine());
                        requestData = JsonSerializer.Serialize(new Request { Command = "Delete", Id = deleteId });
                        break;
                    case COMMANDS.EXIT:
                    case "5":
                        Work = false;
                        break;
                    default:
                        Console.WriteLine("Unknown command.");
                        Console.ReadKey();
                        continue;
                }

                if (requestData == String.Empty) continue;
                string responseData = GetTheServerResponse(client, requestData);

                // Deserialize and print the response
                try
                {
                    var students = new List<Student>();
                    switch (command)
                    {
                        case COMMANDS.GET:
                        case "0":
                            View.Table(JsonSerializer.Deserialize<List<Student>>(responseData));
                            break;
                        case COMMANDS.GET_BY_ID:
                        case "1":
                            View.RecordFromTable(JsonSerializer.Deserialize<Student>(responseData));
                            break;
                        default:
                            Console.WriteLine($"Server response: {responseData}");
                            break;
                    }
                }
                catch (JsonException)
                {
                    Console.WriteLine("JsonException");
                    Console.WriteLine($"Server response: {responseData}");
                }
                Console.ReadKey();
            }

            client.Close();
        }

        private static string GetTheServerResponse(UdpClient client, string requestData)
        {
            byte[] sendBytes = Encoding.UTF8.GetBytes(requestData);
            client.Send(sendBytes, sendBytes.Length);

            IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBytes = client.Receive(ref serverEP);
            string responseData = Encoding.UTF8.GetString(receiveBytes);

            return responseData;
        }

        private static Student? CreateNewRecord(Student? newRecord = default)
        {
            if (EqualityComparer<Student>.Default.Equals(newRecord, default(Student)))
            {
                newRecord = Activator.CreateInstance<Student>();
            }

            PropertyInfo[] properties = typeof(Student).GetProperties();

            //Dictionary<Type, Delegate> inputMethods = new()
            //{
            //    { typeof(string), new Input<string>(view.InputString) },
            //    { typeof(int), new Input<int>(view.InputInt) },
            //    { typeof(bool), new Input<bool>(view.InputBool) }
            //};

            List<string> menu = new List<string>(properties.Select(p => p.Name));

            menu.Add("Сохранить изменения");
            menu.Add("Отмена");

            bool cycle = true;

            while (cycle)
            {
                View.PrintObjectProperties(newRecord);

                View.Menu(menu, "Что вы хотите радактировать?", false);

                int input = View.InputInt();

                if (input >= 0 && input <= menu.Count - 3)
                {
                    PropertyInfo property = properties[input];

                    var value = Validator.ConvertToType(property.PropertyType, View.InputString($"Введите {property.Name}"));

                    try
                    {
                        property.SetValue(newRecord, value);
                    }
                    catch (Exception ex) { View.Error(ex.Message); }
                }
                else if (input == menu.Count - 2)
                {
                    if (newRecord.Valid())
                        //(IsObjectFullyInitialized(newRecord))
                    {
                        View.Massage("Сохранено");
                        cycle = false;
                        return newRecord;
                    }
                    else
                    {
                        View.Error("Введены не все данные");
                    }
                }
                else if (input == menu.Count - 1)
                {
                    View.Massage("Отмена...");
                    return null;
                }
                else
                {
                    View.Error("Некорректный ввод, попробуйте снова");
                }
            }

            return newRecord;
        }

        private static bool IsObjectFullyInitialized<T>(T obj)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (property.IsDefined(typeof(RequiredAttribute), false))
                {
                    object? value = property.GetValue(obj);
                    if (value == null || (value is string strValue && string.IsNullOrWhiteSpace(strValue)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        static class COMMANDS
        {
            public const string GET = "GET";
            public const string GET_BY_ID = "GETBYID";
            public const string ADD = "ADD";
            public const string UPDATE = "UPDATE";
            public const string DELETE = "DELETE";
            public const string EXIT = "EXIT";
        }
    }

    public class Request
    {
        public string Command { get; set; }
        public int Id { get; set; }
        public string Payload { get; set; }
    }
}
