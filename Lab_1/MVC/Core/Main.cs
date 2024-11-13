using ArchitectureOfInformationSystems.MVC.Model;
using ArchitectureOfInformationSystems.MVC.Model.CSVModel;
using ArchitectureOfInformationSystems.MVC.Model.Entity;
using ArchitectureOfInformationSystems.MVC.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Validator = ArchitectureOfInformationSystems.MVC.Model.Validator;

namespace ArchitectureOfInformationSystems.MVC.Core
{
    public class Main <T> where T : class, new()
    {
        CSVModel<T> model;
        private View.View view;
        private FunctionList functions;

        public Main(string filePath = @"D:\VS\ArchitectureOfInformationSystems\Data\data.CSV")
        {
            //_ = new MenuHander<Student>(filePath);
            view = new View.View();

            try
            {
                model = new CSVModel<T>(filePath);
            }
            catch (Exception e) { 
                view.Error(e.Message);
                return;
            }

            functions = new FunctionList("Главное меню");

            functions.AddFunction("Вывести все записи", OutputAll);
            functions.AddFunction("Вывести запись по номеру", OutputByNumber);
            functions.AddFunction("Записать данные в файл", WriteData);
            functions.AddFunction("Удалить запись из файла", DeleteRecordFromFile);
            functions.AddFunction("Добавить запись в файл", AddAnEntry);
            functions.AddFunction("Сгенероровать записи", AddRandomEntries);
            functions.AddFunction("Выход", Exit);
            
            SelectFromTheMenu(functions);
        }

        #region main menu
        /// <summary>
        /// Вывести все записи
        /// </summary>
        private void OutputAll()
        {
            view.Table(model.GetValues());
            ExitToTheMenu();
        }

        /// <summary>
        /// Вывести запись по номеру
        /// </summary
        private void OutputByNumber()
        {
            List<T> records = model.GetValues();

            if (records.Count == 0)
            {
                view.Massage("Нет записей.");
                ExitToTheMenu();
                return;
            }

            while (true) 
            {
                view.Table(records);
                int x = view.InputInt("Введите номер строки");
                
                if (x >= 0 && x <= records.Count - 1)
                {
                    view.PrintObjectProperties(records[x]);
                    break;
                }
                else
                {
                    view.Error("Недопустимый номер записи или некорректный ввод. Попробуйте снова.");
                }

            };

            ExitToTheMenu();
        }

        //Перезапись данных в файл !!!!!!!!!!!!!
        /// <summary>
        /// Записать данные в файл
        /// </summary>
        private void WriteData()
        {
            List<T> records = new();
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<string> menu = new List<string>() { "Число - заменить запись", "+ - добавить новую запись", "- - удалить запись", "save - сохранить записи", "cancel - отмена", "rand - сгенерировать(если есть функционал)" };

            string menuInput;
            bool cycle = true;

            while (cycle)
            {
                view.Table(records);
                view.Menu(menu, "Меню редактирования файлов", false);
                menuInput = view.InputString();

                if (menuInput == "+")
                {
                    T? record = CreateNewRecord();
                    if (record != null)
                    {
                        records.Add(record);
                    }
                }
                else if (menuInput == "-")
                {
                    records = DeleteRecord(records);
                }
                else if (menuInput == "save")
                {
                    view.Massage("Вы точно хотите сохранить файл?");
                    if (view.InputBool())
                    {
                        model.OverwritingTable(records);
                        cycle = false;

                        break;
                    }
                }
                else if (menuInput == "cancel")
                {
                    cycle = false;
                    break;
                }

                else if (menuInput == "rand")
                {
                    if (typeof(T) == typeof(Student))
                    {
                        (records as List<Student>).AddRange(Student.AddRandomEntries(1));
                    }
                    else { Console.WriteLine("Не, нет"); }
                }
                if (Int32.TryParse(menuInput, out int x) && x >= 0 && x <= records.Count - 1)
                {
                    T? record = CreateNewRecord(records[x]);
                    if (record != null)
                    {
                        records[x] = record;
                    }
                }
            }
            ExitToTheMenu();
        }

        /// <summary>
        /// Удалить запись из файла
        /// </summary>
        public void DeleteRecordFromFile()
        {
            List<T> records = model.GetValues();
            if (records.Count == 0)
            {
                view.Massage("Нет записей для удаления.");
                ExitToTheMenu();
                return;
            }

            while (true)
            {
                view.Table(records);
                int indexToDelete = -1;

                view.TextOutput("Нажмите на любую клавишу чтобы продолжить (ESC для отмены):");
                var key = Console.ReadKey();
                Console.Out.Flush();

                if (key.Key == ConsoleKey.Escape)
                {
                    view.Massage("Отменено.");
                    break;
                }

                indexToDelete = view.InputInt();
                if (indexToDelete >= 0 && indexToDelete < records.Count)
                {
                    model.RemoveEntry(indexToDelete);
                    view.Massage("Запись удалена.");
                    break;
                }
                else
                {
                    view.Error("Недопустимый номер записи.");
                }
            }
            ExitToTheMenu();
        }

        /// <summary>
        /// Удалить запись
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private List<T> DeleteRecord(List<T> records)
        {
            if (records.Count == 0)
            {
                view.Massage("Нет записей для удаления.");
                ExitToTheMenu();
                return records;
            }


            while (true)
            {
                view.Table(records);
                int indexToDelete = -1;

                view.TextOutput("Нажмите на любую клавишу чтобы продолжить (ESC для отмены):");
                var key = Console.ReadKey();
                Console.Out.Flush();

                if (key.Key == ConsoleKey.Escape)
                {
                    view.Massage("Отменено.");
                    return records;
                }

                indexToDelete = view.InputInt();
                if (indexToDelete >= 0 && indexToDelete < records.Count)
                {
                    records.RemoveAt(indexToDelete);
                    view.Massage("Запись удалена.");
                    break;
                }
                else
                {
                    view.Error("Недопустимый номер записи.");
                }

            }
            return records;
        }

        /// <summary>
        /// Добавить запись в файл
        /// </summary>
        private void AddAnEntry()
        {
            T? record = CreateNewRecord();
            if (!EqualityComparer<T>.Default.Equals(record, default(T))) model.AddValues(new List<T>() { record });
            ExitToTheMenu();
        }

        /// <summary>
        /// Проверка на целосность параметров
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool IsObjectFullyInitialized<T>(T obj)
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

        /// <summary>
        /// Выход в меню
        /// </summary>
        private void ExitToTheMenu()
        {
            view.Massage("Нажмите на любую клавишу чтобы выйти в меню");
            Console.ReadKey();
            SelectFromTheMenu(functions);
        }

        /// <summary>
        /// Выход из приложения
        /// </summary>
        private void Exit()
        {
            view.Massage("Нажмите на любую клавишу чтобы выйти");
            Console.ReadKey();
            Environment.Exit(0);
        }

        #endregion main menu

        /// <summary>
        /// Выбрать функцию из списка
        /// </summary>
        /// <param name="functionList">список функций</param>
        private void SelectFromTheMenu(FunctionList functionList)
        {
            while (true)
            {
                view.Menu(functions.GetFunNames());
                int input = view.InputInt("Введите номер операции");

                try
                {
                    functionList.ExecuteFunction(input);
                }
                catch (Exception e)
                {
                    view.Error(e.Message);
                }

            }
        }

        /// <summary>
        /// Заполнить запись
        /// </summary>
        /// <param name="newRecord"></param>
        /// <returns></returns>
        private T? CreateNewRecord(T? newRecord = default)
        {
            if (EqualityComparer<T>.Default.Equals(newRecord, default(T)))
            {
                newRecord = Activator.CreateInstance<T>();
            }

            PropertyInfo[] properties = typeof(T).GetProperties();

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
                view.PrintObjectProperties(newRecord);

                view.Menu(menu, "Что вы хотите радактировать?", false);

                int input = view.InputInt();

                if (input >= 0 && input <= menu.Count - 3)
                {
                    PropertyInfo property = properties[input];

                    try
                    {
                        var value = Validator.ConvertToType(property.PropertyType, view.InputString($"Введите {property.Name}"));

                        property.SetValue(newRecord, value);
                    }
                    catch (Exception ex) { view.Error(ex.Message); }
                }
                else if (input == menu.Count - 2)
                {
                    if (IsObjectFullyInitialized(newRecord))
                    {
                        view.Massage("Сохранено");
                        cycle = false;
                        return newRecord;
                    }
                    else
                    {
                        view.Error("Введены не все данные");
                    }
                }
                else if (input == menu.Count - 1)
                {
                    view.Massage("Отмена...");
                    return default;
                }
                else
                {
                    view.Error("Некорректный ввод, попробуйте снова");
                }
            }

            return newRecord;
        }

        private void AddRandomEntries()
        {
            if (typeof(T) == typeof(Student))
            {
                List<Student> students = Student.AddRandomEntries(10);
                (model as CSVModel<Student>).AddValues(students);
            }
            ExitToTheMenu();
        }
    }


    #region Function List
    /// <summary>
    /// Информация о функции (Название и функция)
    /// </summary>
    public class FunctionInfo
    {
        public string Name { get; private set; }
        public FunctionDelegate Function { get; private set; }

        public delegate void FunctionDelegate();

        public FunctionInfo(string name, FunctionDelegate function)
        {
            Name = name;
            Function = function;
        }
    }

    public class FunctionList
    {
        public FunctionList(string name)
        {
            Name = name;
            Functions = new List<FunctionInfo> ();
        }

        private List<FunctionInfo> Functions;

        public string Name {
            get;
            private set;
        }

        public void AddFunction(string functionName, FunctionInfo.FunctionDelegate function)
        {
            Functions.Add(new FunctionInfo(functionName, function));
        }

        public void RemoveFunction(string functionName)
        {
            Functions.RemoveAll(x => x.Name == functionName);
        }
        public void RemoveFunction(int functionIndex)
        {
            Functions.RemoveAt(functionIndex);
        }


        public void ExecuteFunction(int index)
        {
            if (index >= 0 && index < Functions.Count)
            {
                Functions[index].Function.Invoke();
            }
            else { throw new Exception("Недопустимый номер функции."); }
        }

        public List<string> GetFunNames()
        {
            List<string> names = new ();
            foreach (FunctionInfo f in Functions) {
                names.Add(f.Name);
            }
            return names;
        }
    }
    #endregion Function List
}
