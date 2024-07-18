using System;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

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

            while (true)
            {
                //Console.WriteLine("Enter command (Get, GetById, Add, Update, Delete, Exit):");
                View.Menu(menu, "Enter command", false);
                string command = Console.ReadLine();

                if (command.Equals("Exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                string requestData = string.Empty;

                switch (command)
                {
                    case "Get":
                    case "0":
                        requestData = JsonSerializer.Serialize(new Request { Command = "Get" });
                        break;
                    case "GetById":
                    case "1":
                        Console.WriteLine("Enter ID:");
                        int getId = int.Parse(Console.ReadLine());
                        requestData = JsonSerializer.Serialize(new Request { Command = "GetById", Id = getId });
                        break;
                    case "Add":
                    case "2":
                        Student newStudent = GetStudentFromInput();
                        requestData = JsonSerializer.Serialize(new Request { Command = "Add", Payload = JsonSerializer.Serialize(newStudent) });
                        break;
                    case "Update":
                    case "3":
                        Console.WriteLine("Enter ID:");
                        int updateId = int.Parse(Console.ReadLine());
                        Student updatedStudent = GetStudentFromInput();
                        requestData = JsonSerializer.Serialize(new Request { Command = "Update", Id = updateId, Payload = JsonSerializer.Serialize(updatedStudent) });
                        break;
                    case "Delete":
                    case "4":
                        Console.WriteLine("Enter ID:");
                        int deleteId = int.Parse(Console.ReadLine());
                        requestData = JsonSerializer.Serialize(new Request { Command = "Delete", Id = deleteId });
                        break;
                    default:
                        Console.WriteLine("Unknown command.");
                        Console.ReadKey();
                        continue;
                }

                byte[] sendBytes = Encoding.UTF8.GetBytes(requestData);
                client.Send(sendBytes, sendBytes.Length);

                IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, 0);
                byte[] receiveBytes = client.Receive(ref serverEP);
                string responseData = Encoding.UTF8.GetString(receiveBytes);

                // Deserialize and print the response
                try
                {
                    var students = new List<Student>();
                    switch (command)
                    {
                        case "Get":
                        case "0":
                            View.Table(JsonSerializer.Deserialize<List<Student>>(responseData));
                            break;
                        case "GetById":
                        case "1":
                            View.RecordFromTable(JsonSerializer.Deserialize<Student>(responseData));
                            break;
                        default:
                            Console.WriteLine($"Server response: {responseData}");
                            break;
                    }

                    //if (command.Equals("Get", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    var students = JsonSerializer.Deserialize<List<Student>>(responseData);
                    //    Console.WriteLine("Server response:");
                    //    foreach (var student in students)
                    //    {
                    //        Console.WriteLine($"Name: {student.Name}, LastName: {student.LastName}, Age: {student.Age}, IsStudent: {student.IsStudent}");
                    //    }
                    //    View.Table(students);
                    //}
                    //else if (command.Equals("GetById", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    var student = JsonSerializer.Deserialize<Student>(responseData);
                    //    Console.WriteLine("Server response:");
                    //    Console.WriteLine($"Name: {student.Name}, LastName: {student.LastName}, Age: {student.Age}, IsStudent: {student.IsStudent}");
                    //}
                    //else
                    //{
                    //    Console.WriteLine($"Server response: {responseData}");
                    //}
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

        private static Student GetStudentFromInput()
        {
            Console.WriteLine("Enter name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter last name:");
            string lastName = Console.ReadLine();
            Console.WriteLine("Enter age:");
            int age = int.Parse(Console.ReadLine());
            Console.WriteLine("Is student (true/false):");
            bool isStudent = bool.Parse(Console.ReadLine());

            return new Student(name, lastName, age, isStudent);
        }
    }

    public class Request
    {
        public string Command { get; set; }
        public int Id { get; set; }
        public string Payload { get; set; }
    }

    public class Student
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public bool IsStudent { get; set; }

        public Student() { }

        public Student(string name, string lastName, int age, bool isStudent)
        {
            Name = name;
            LastName = lastName;
            Age = age;
            IsStudent = isStudent;
        }
    }
}
