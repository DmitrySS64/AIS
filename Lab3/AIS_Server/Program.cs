using Newtonsoft.Json;
using server.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class Server
    {
        private const int listenPort = 11000;
        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "data.csv");
        //private static List<Dictionary<string, string>> records = new();
        private static CSVModel<Student> studentModel;

        public Server()
        {
            studentModel = new CSVModel<Student>(filePath);
        }

        private static void StartListener()
        {
            studentModel = new CSVModel<Student>(filePath);
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            Console.WriteLine("UDP Server is listening on port {0}", listenPort);

            try
            {
                while (true)
                {
                    byte[] bytes = listener.Receive(ref groupEP);
                    string receivedData = Encoding.UTF8.GetString(bytes);
                    Console.WriteLine($"Received: {receivedData} from {groupEP}");

                    var response = HandleRequest1(receivedData);
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);

                    listener.Send(responseBytes, responseBytes.Length, groupEP);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }
        }

        private static string HandleRequest1(string requestData)
        {
            var request = JsonConvert.DeserializeObject<Request>(requestData);
            var students = studentModel.GetValues();
            switch (request.Command)
            {
                case "Get":
                    return JsonConvert.SerializeObject(students);
                case "GetById":
                    if (request.Id >= students.Count || request.Id < 0)
                    {
                        return "NotFound";
                    }
                    return JsonConvert.SerializeObject(students[request.Id]);
                case "Add":
                    var newStudent = JsonConvert.DeserializeObject<Student>(request.Payload);
                    var entryFields = new string[] { newStudent.Name, newStudent.LastName, newStudent.Age.ToString(), newStudent.IsStudent.ToString() };
                    studentModel.AddEntry(entryFields);
                    return "OK";
                case "Update":
                    if (request.Id >= students.Count || request.Id < 0)
                    {
                        return "NotFound";
                    }
                    var updatedStudent = JsonConvert.DeserializeObject<Student>(request.Payload);
                    var updateFields = new string[] { updatedStudent.Name, updatedStudent.LastName, updatedStudent.Age.ToString(), updatedStudent.IsStudent.ToString() };
                    studentModel.EditEntry(request.Id, updateFields);
                    return "OK";
                case "Delete":
                    if (request.Id >= students.Count || request.Id < 0)
                    {
                        return "NotFound";
                    }
                    studentModel.RemoveEntry(request.Id);
                    return "OK";
                default:
                    return "Unknown command";
            }
        }

        static void Main(string[] args)
        {
            //new Server(11000);
            //Server server = new();
            StartListener();
        }
    }

    public class Request
    {
        public string Command { get; set; }
        public int Id { get; set; }
        public string Payload { get; set; }
    }
}

