using Lab2_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab2_server
{
    class Server
    {
        private const int listenPort = 11000;
        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "data.csv");
        private static List<Dictionary<string, string>> records = new();
        private static CSVModel<Student>? studentModel;

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
            var request = JsonSerializer.Deserialize<Request>(requestData);
            var students = studentModel.GetValues();
            switch (request.Command)
            {
                case "Get":
                    return JsonSerializer.Serialize(students);
                case "GetById":
                    if (request.Id >= students.Count || request.Id < 0)
                    {
                        return "NotFound";
                    }
                    return JsonSerializer.Serialize(students[request.Id]);
                case "Add":
                    var newStudent = JsonSerializer.Deserialize<Student>(request.Payload);
                    var entryFields = new string[] { newStudent.Name, newStudent.LastName, newStudent.Age.ToString(), newStudent.IsStudent.ToString() };
                    studentModel.AddEntry(entryFields);
                    return "OK";
                case "Update":
                    if (request.Id >= students.Count || request.Id < 0)
                    {
                        return "NotFound";
                    }
                    var updatedStudent = JsonSerializer.Deserialize<Student>(request.Payload);
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

        private static Dictionary<string, object> HandleRequest(Dictionary<string, object> request)
        {
            string action = request["action"].ToString();

            switch (action)
            {
                case "get_all_records":
                    {
                        return GetAllRecords();
                    }
                case "get_record_by_id":
                    {
                        int id = Convert.ToInt32(request["id"]);
                        return GetRecordByID(id);
                    }
                case "add_record":
                    {
                        var record = JsonSerializer.Deserialize<Dictionary<string, string>>(request["content"].ToString());
                        return AddRecord(record);

                    }
                case "add_records":
                    {
                        var records = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(request["content"].ToString());
                        return AddRecords(records);
                    }
                case "remove_record":
                    {
                        int id = Convert.ToInt32(request["id"]);
                        if (RemoveRecord(id))
                        {
                            return new Dictionary<string, object>
                            {
                                { "type", "success" },
                                { "message", "Запись удалена." }
                            };
                        }
                        else
                        {
                            return new Dictionary<string, object>
                            {
                                { "type", "error" },
                                { "message", "Ошибка при удалении" }
                            };
                        }
                    }
                default:
                    {
                        return new Dictionary<string, object>
                        {
                            { "type", "error" },
                            { "message", "Неизвестный запрос." }
                        };
                    }
            }
        }

        private static Dictionary<string, object> GetAllRecords()
        {
            return new Dictionary<string, object>
            {
                { "type", "table"},
                { "content", records }
            };

        }

        private static Dictionary<string, object> GetRecordByID(int id)
        {
            if (id >= 0 && id < records.Count)
            {
                return new Dictionary<string, object> {
                    { "type", "record" },
                    { "content", records[id] }
                };
            }
            return new Dictionary<string, object>
            {
                { "type", "error" },
                { "message", "Запись с указанным ID не найдена." }
            };
        }

        private static Dictionary<string, object> AddRecord(Dictionary<string, string> record)
        {
            records.Add(record);
            //SaveRecordsToFile();
            return new Dictionary<string, object>
            {
                { "type", "success" },
                { "message", "Запись добавлена." }
            };
            //return new Dictionary<string, object>
            //{
            //    { "type", "error" },
            //    { "message", "Добавить запись не получилось" }
            //};
        }

        private static Dictionary<string, object> AddRecords(List<Dictionary<string, string>> records)
        {
            records.AddRange(records);
            //SaveRecordsToFile();
            return new Dictionary<string, object>
            {
                { "type", "success" },
                { "message", "Записи добавлены." }
            };
        }
        private static bool RemoveRecord(int id)
        {
            if (id >= 0 && id < records.Count)
            {
                records.RemoveAt(id);
                //SaveRecordsToFile();
                return true;
            }
            return false;
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

