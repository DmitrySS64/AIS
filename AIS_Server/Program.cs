using AIS_Server.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NLog;
using NLog.Config;

namespace AIS_Server
{
    class Server
    {
        private const int listenPort = 11000;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static readonly Dictionary<string, Type> availableClasses = new Dictionary<string, Type>
        {
            { "student", typeof(Students) },
            { "teachers", typeof(Teachers) }
            
        };

        private static void StartListener()
        {
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            logger.Info($"UDP Server is listening on port {listenPort}");

            try
            {
                while (true)
                {
                    byte[] bytes = listener.Receive(ref groupEP);
                    string receivedData = Encoding.UTF8.GetString(bytes);
                    logger.Info($"Received: {receivedData} from {groupEP}");

                    var response = HandleRequest(receivedData);
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);

                    listener.Send(responseBytes, responseBytes.Length, groupEP);
                    logger.Info($"Response sent to {groupEP}: {response}");
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Error occurred in UDP listener");
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
                logger.Info("UDP listener closed");
            }
        }

        private static string HandleRequest(string requestData)
        {
            if (string.IsNullOrWhiteSpace(requestData))
            {
                logger.Warn("Received empty or whitespace request data");
                return "Invalid request data";
            }

            Request request;
            try
            {
                request = JsonConvert.DeserializeObject<Request>(requestData);
            }
            catch (Exception ex)
            {
                logger.Error($"Error deserializing request: {ex.Message}");
                return "Invalid request format";
            }

            if (request.Command == "GetClasses")
            {
                // Возвращает список доступных классов
                return JsonConvert.SerializeObject(availableClasses.Keys);
            }

            if (request == null || string.IsNullOrWhiteSpace(request.ClassName) || string.IsNullOrWhiteSpace(request.Command))
            {
                logger.Warn("Received invalid request format");
                return "Invalid request format";
            }


            if (!availableClasses.TryGetValue(request.ClassName.ToLower(), out Type classType))
            {
                logger.Warn($"Class not found: {request.ClassName}");
                return "Class not found";
            }

            string response;

            try
            {
                var methodInfo = typeof(Controller<>).MakeGenericType(classType);
                if (methodInfo == null)
                {
                    logger.Warn("Method not found");
                    return "Method not found";
                }
                switch (request.Command.ToLower())
                {
                    case "getform":
                        return FormController.GetFormDescription(request.ClassName);
                    case "get":
                        response = (string)methodInfo
                            .GetMethod("ListObjects")
                            .Invoke(null, null);
                        break;
                    case "getbyid":
                        response = (string)methodInfo
                            .GetMethod("GetObjectById")
                            .Invoke(null, new object[] { request.Id });
                        break;
                    case "add":
                        var newObj = JsonConvert.DeserializeObject(request.Payload, classType);
                        response = (string)methodInfo
                            .GetMethod("AddObject")
                            .Invoke(null, new object[] { newObj });
                        break;
                    case "update":
                        var updatedObj = JsonConvert.DeserializeObject(request.Payload, classType);
                        response = (string)methodInfo
                            .GetMethod("UpdateObject")
                            .Invoke(null, new object[] { request.Id, updatedObj });
                        break;
                    case "delete":
                        response = (string)methodInfo
                            .GetMethod("DeleteObject")
                            .Invoke(null, new object[] { request.Id });
                        break;
                    default:
                        logger.Warn($"Unknown command: {request.Command}");
                        response = "Unknown command";
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error processing request");
                response = $"Error processing request: {ex.Message}";
            }

            return response;
        }

        static void Main(string[] args)
        {
            LogManager.Configuration = new XmlLoggingConfiguration("NLog.config");
            logger.Info("Server starting...");
            StartListener();
        }
    }

    public class Request
    {
        public string ClassName { get; set; }
        public string Command { get; set; }
        public int Id { get; set; }
        public string Payload { get; set; }
    }
}