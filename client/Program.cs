using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;
using client.View;
using System;
using System.Reflection;

namespace client
{
    class Client
    {
        private static List<string> entities = new List<string>();
        private static int indexCurrentEntity = -1;
        private static UdpClient client;
        private static IPEndPoint serverEP;

        private static Menu MainMenu = new Menu(
            menu: 
            [
                new MenuItem("List entities", ListEntities),
                new MenuItem("View entity by ID", ViewEntityById),
                new MenuItem("Add new entity", AddEntity),
                new MenuItem("Update entity by ID", UpdateEntityById),
                new MenuItem("Delete entity by ID", DeleteEntityById),
                new MenuItem("Switch current entity", SwitchEntity),
                new MenuItem("Exit", () => Environment.Exit(0), ConsoleKey.Escape)
            ], 
            title: "Main Menu"
        );

        private static void Main(string[] args)
        {
            client = new UdpClient();
            serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);

            while (true)
            {
                try
                {
                    LoadEntities();

                    while (true)
                    {
                        ShowMainMenu();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    WaitForUserInput();
                }
            }
        }

        /// <summary>
        /// Получить все сущности
        /// </summary>
        private static void LoadEntities()
        {
            SendRequest(new Request { Command = Commands.getClasses });
            var response = ReceiveResponse();
            entities = JsonConvert.DeserializeObject<List<string>>(response);
            indexCurrentEntity = entities.Count > 0 ? 0 : -1;
            Console.WriteLine($"Available classes: {string.Join(", ", entities)}");
        }

        /// <summary>
        /// Вызов главного меню 
        /// </summary>
        private static void ShowMainMenu()
        {
            string currentEntity = indexCurrentEntity != -1 ? entities[indexCurrentEntity] : "none";
            Console.Clear();
            Console.WriteLine($"Current entity: {currentEntity} (available {entities.Count})");
            MainMenu.UseMenu();

            WaitForUserInput();
        }

        #region CRUD
        /// <summary>
        /// Показать список записей
        /// </summary>
        private static void ListEntities()
        {
            Console.Clear();
            if (indexCurrentEntity == -1)
            {
                Console.WriteLine("No entity selected.");
                return;
            }

            SendRequest(new Request { ClassName = entities[indexCurrentEntity], Command = Commands.get });
            var response = ReceiveResponse();
            var objects = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response);

            ConsoleTable.PrintTable(objects);
        }

        /// <summary>
        /// Поиск записи
        /// </summary>
        private static void ViewEntityById()
        {
            Console.Clear();
            if (indexCurrentEntity == -1)
            {
                Console.WriteLine("No entity selected.");
                return;
            }

            Console.WriteLine("Enter ID:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                SendRequest(new Request { ClassName = entities[indexCurrentEntity], Command = Commands.getById, Id = id });
                var response = ReceiveResponse();
                var obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
                ConsoleTable.PrintTableRecord(obj);
            }
            else
            {
                Console.WriteLine("Invalid ID");
            }
        }

        /// <summary>
        /// Добавить пользователя
        /// </summary>
        private static void AddEntity()
        {
            Console.Clear();
            HandleAddOrUpdateEntity(Commands.add);
        }

        /// <summary>
        /// Редактировать пользователя
        /// </summary>
        private static void UpdateEntityById()
        {
            Console.Clear();
            if (indexCurrentEntity == -1)
            {
                Console.WriteLine("No entity selected.");
                return;
            }

            Console.WriteLine("Enter ID:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                HandleAddOrUpdateEntity(Commands.update, id);
            }
            else
            {
                Console.WriteLine("Invalid ID");
            }
        }

        /// <summary>
        /// Удалить запись по ID
        /// </summary>
        private static void DeleteEntityById()
        {
            Console.Clear();
            if (indexCurrentEntity == -1)
            {
                Console.WriteLine("No entity selected.");
                return;
            }

            Console.WriteLine("Enter ID:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                SendRequest(new Request { ClassName = entities[indexCurrentEntity], Command = Commands.delete, Id = id });
                var response = ReceiveResponse();
                var objects = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
                Console.WriteLine(response);
            }
            else
            {
                Console.WriteLine("Invalid ID");
            }
        }

        /// <summary>
        /// Сменить таблицу
        /// </summary>
        private static void SwitchEntity()
        {
            Console.Clear();
            Console.WriteLine("Available classes:");
            for (int i = 0; i < entities.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {entities[i]}");
            }

            Console.WriteLine("Enter the number of the class to switch to:");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= entities.Count)
            {
                indexCurrentEntity = index - 1;
                Console.WriteLine($"Switched to {entities[indexCurrentEntity]}");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        /// <summary>
        /// Вызов пользовательского интерфейса для редактирования записи
        /// </summary>
        /// <param name="form">Форма для записи</param>
        /// <param name="existingData">Начальное значение данных</param>
        /// <returns></returns>
        private static Dictionary<string, object> CollectEntityData(FormDescription form, Dictionary<string, object> existingData)
        {
            Dictionary<string, object> data = new(existingData);

            while (true) {
                Console.Clear();
                ConsoleTable.PrintTableRecord(data);
                for (int i = 0; i < form.Fields.Count; i++)
                {
                    FormField field = form.Fields[i];
                    string currentValue = data.ContainsKey(field.Name) ? data[field.Name]?.ToString() : "не задано";

                    Console.WriteLine($"{i + 1}. {field.Label} ({field.Type} - Default: {field.DefaultValue ?? "нет значения"}) : Текущее значение: {currentValue}");
                }
                Console.WriteLine("\nВыберите номер поля для редактирования или нажмите Esc для завершения:");
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    return data; // Завершить редактирование
                }

                if (int.TryParse(key.KeyChar.ToString(), out int index) && index >= 1 && index <= form.Fields.Count)
                {
                    FormField selectedField = form.Fields[index - 1];
                    Console.WriteLine($"\nВведите новое значение для {selectedField.Label} (оставьте пустым для значения по умолчанию): ");
                    string input = Console.ReadLine();

                    object value = string.IsNullOrEmpty(input) ? selectedField.DefaultValue : ConvertToFieldType(input, selectedField.Type);
                    data[selectedField.Name] = value;
                }
                else
                {
                    Console.WriteLine("Неверный номер. Попробуйте ещё раз.");
                }
            }
        }

        /// <summary>
        /// Добавить или обновить запись
        /// </summary>
        /// <param name="command">Command.add || Command.update</param>
        /// <param name="entityId">Id записи если нужно отредактировать</param>
        private static void HandleAddOrUpdateEntity(string command, int? entityId = null)
        {
            Dictionary<string, object> entityData = new Dictionary<string, object>();

            if (entityId.HasValue) {
                SendRequest(new Request { ClassName = entities[indexCurrentEntity], Command = Commands.getById, Id = entityId.Value });
                var entityResponse = ReceiveResponse();
                entityData = JsonConvert.DeserializeObject<Dictionary<string, object>>(entityResponse);
            }

            // Получаем форму сущности с сервера
            SendRequest(new Request { ClassName = entities[indexCurrentEntity], Command = Commands.getForm });
            var formResponse = ReceiveResponse();
            var form = JsonConvert.DeserializeObject<FormDescription>(formResponse);

            // Собираем данные сущности
            var data = CollectEntityData(form, entityData);

            if (data == null)
            {
                Console.WriteLine("Добавление/редактирование отменено.");
                return;
            }

            // Подтверждение или отмена изменений
            Console.WriteLine("\nПодтвердите изменения (Enter для подтверждения, Esc для отмены):");
            ConsoleKeyInfo confirmationKey = Console.ReadKey(true);
            if (confirmationKey.Key == ConsoleKey.Escape)
            {
                Console.WriteLine("Операция отменена.");
                return;
            }

            // Формируем запрос на добавление или обновление
            var request = new Request
            {
                ClassName = entities[indexCurrentEntity],
                Command = command,
                Id = entityId ?? 0,
                Payload = JsonConvert.SerializeObject(data)
            };

            SendRequest(request);
            var response = ReceiveResponse();
            Console.WriteLine($"Server response: {response}");
        }
        #endregion CRUD


        /// <summary>
        /// Конвертирование ввода под нужный тип 
        /// </summary>
        /// <param name="input">ввод</param>
        /// <param name="type">тип данных</param>
        /// <returns></returns>
        private static object ConvertToFieldType(string input, string type)
        {
            switch (type.ToLower())
            {
                case "text":
                    return input;
                case "number":
                    if (int.TryParse(input, out int intValue))
                    {
                        return intValue;
                    }
                    break;
                case "date":
                    if (DateTime.TryParse(input, out DateTime dateValue))
                    {
                        return dateValue;
                    }
                    break;
                case "checkbox":
                    if (bool.TryParse(input, out bool boolValue))
                    {
                        return boolValue;
                    }
                    break;
            }

            Console.WriteLine("Неверный формат значения. Попробуйте ещё раз.");
            return null;
        }

        /// <summary>
        /// Отправить сообщение серверу
        /// </summary>
        /// <param name="request"></param>
        private static void SendRequest(Request request)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
            client.Send(data, data.Length, serverEP);
        }

        /// <summary>
        /// Получить ответ сервера
        /// </summary>
        /// <returns></returns>
        private static string ReceiveResponse()
        {
            var receivedData = client.Receive(ref serverEP);
            return Encoding.UTF8.GetString(receivedData);
        }
        private static void WaitForUserInput()
        {
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey(true);
        }
    }

    /// <summary>
    /// Сообщение сервера
    /// </summary>
    public class Request
    {
        public string ClassName { get; set; } //имя таблицы
        public string Command { get; set; } //команда
        public int Id { get; set; } //id записи
        public string Payload { get; set; } //параметры
    }

    public class FormField
    {
        public string Name { get; set; }
        public string Type { get; set; } // Например, "text", "number", "date", "checkbox" и т.д.
        public bool IsRequired { get; set; }
        public string Label { get; set; }
        public object DefaultValue { get; set; } // Значение по умолчанию, если применимо
    }

    /// <summary>
    /// Хранит получаемый-отправляемый класс и его полями
    /// </summary>
    public class FormDescription
    {
        public string ClassName { get; set; }
        public List<FormField> Fields { get; set; }
    }
}
