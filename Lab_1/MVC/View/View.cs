using ArchitectureOfInformationSystems.MVC.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureOfInformationSystems.MVC.View
{
    public class View
    {
        public View() { }

        public void Menu(List<string> menu, string menuName = "Меню", bool clearView = true)
        {
            if (clearView) Clear();
            Console.WriteLine($"{menuName}");
            int i = 0;
            foreach (var item in menu)
            {
                Console.Write(i.ToString() + Constants.Numerator);
                Console.WriteLine(item.ToString());
                i++;
            }
        }

        public void Table<T>(List<T> records)
        {
            Clear();

            if (records.Count == 0)
            {
                TextOutput("Нет записей.");
                return;
            }

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            // Вывод заголовков столбцов
            foreach (var property in properties)
            {
                Console.Write($"{property.Name}\t");
            }
            Console.WriteLine();

            int i = -1;
            foreach (var record in records)
            {
                i++;
                Console.Write($"{i})\t");
                foreach (var property in properties)
                {
                    object value = property.GetValue(record);
                    Console.Write($"{value}\t");
                }
                Console.WriteLine();
            }
        }



        public void Error(string s)
        {
            Console.Write($"{Constants.Error}{s}\n");
            Console.ReadKey();
        }

        public void Clear()
        {
            Console.Clear();
            //Massage("");
        }

        public void Massage(string str)
        {
            Console.WriteLine($"{Constants.Massage}{str}\n");
        }

        public void TextOutput(string str)
        {
            Console.WriteLine($"{Constants.Text}{str}");
        }

        public void PrintObjectProperties(object? obj, bool clearView = true)
        {
            if (clearView) Clear();

            if (obj == null)
            {
                TextOutput("Объект пуст");
                return;
            }

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            Console.WriteLine($"Object of type {type.Name}:");

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);
                Console.WriteLine($"{property.Name}:\t{value}");
            }
        }

        private void Rejection(string massege = "Некорректный ввод, попробуйте снова")
        {
            Console.WriteLine($"{Constants.Rejection}{massege}");
            Console.ReadKey();
        }

        public string InputString(string massage = "Введите текст")
        {
            string? input;

            while (true)
            {
                Massage(massage);
                input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input)) return input;
                Rejection();
            }
        }

        public bool InputBool(string massage = "Введите 0 - False или 1 - True")
        {
            while (true)
            {
                Massage(massage);

                if (Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out int x) && (x == 0 || x == 1))
                {
                    return x == 1;
                }

                Rejection();
            }
        }

        public int InputInt(string massage = "Введите число")
        {
            while (true)
            {
                Massage(massage);

                if (Int32.TryParse(Console.ReadLine(), out int x))
                {
                    return x;
                }

                Rejection();
            }
        }



    }

    public class Constants
    {
        public const string Input = ">>> ",
            Massage = ">> ",
            Rejection = "Исключение: ",
            Error = "Ошибка: ",
            Text = "> ",
            Numerator = ") ";
    }
    
}
