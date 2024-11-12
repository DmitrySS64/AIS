using AngleSharp;
using AngleSharp.Browser;
using AngleSharp.Dom;
using AngleSharp.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Lab_6
{
    internal class Program
    {
        const string urlListAvito = "https://www.avito.ru/tomsk/avtomobili?radius=200&searchRadius=200";
        const string urlAvito = "https://www.avito.ru/tomsk/kollektsionirovanie/printsessa_iz_kinder_dlya_viktorii_4245155571";

        static async Task Main(string[] args)
        {
            Parser parser = new Parser();

            //await parser.ParseProductInfo("https://www.avito.ru/tomsk/kvartiry/3-k._kvartira_843_m_1215_et._4435934929");
            //await parser.ParseProductInfo("https://www.avito.ru/kemerovo/avtomobili/vaz_lada_21099_1.5_mt_2003_250_000_km_4368838924");


            //await parser.ListProducts(url);
            //await parser.ParseProductInfo("https://www.avito.ru/tomsk/kollektsionirovanie/printsessa_iz_kinder_dlya_viktorii_4245155571");

            //await Load2droida(parser, "https://2droida.ru/catalog/umnyy-dom");

            //dbViewer.Main();

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"{ConsoleKey.D1}. Загрузить данные с сайта");
                Console.WriteLine($"{ConsoleKey.D2}. Просмотреть таблицу");
                Console.WriteLine($"{ConsoleKey.Escape}. Выход");
                Console.WriteLine("------------------------");
                var input = Console.ReadKey().Key;

                if (input == ConsoleKey.D1)
                {
                    await Load2droida(parser, "https://2droida.ru/catalog/umnyy-dom");
                    Console.WriteLine("Нажмите Enter чтобы продолжить");
                    Console.ReadLine();
                }
                else if (input == ConsoleKey.D2)
                {
                    dbViewer.Main();
                }
                else if (input == ConsoleKey.Escape)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }

            //await LoadOzon(parser);

            Console.WriteLine("Конец..........");
            Console.ReadKey();
        }

        static async Task Load2droida(Parser parser, string url = "https://2droida.ru/catalog/elektronika")
        {
            List<Product> products = new List<Product>();
            try
            {
                products = await parser.GetProductsSimple2droida(url);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (products.Count > 0)
            {
                Console.WriteLine($"Обнаружено {products.Count} товаров");
                foreach (var product in products)
                {
                    Console.WriteLine(product.Name);
                    Console.WriteLine(product.Price);
                    Console.WriteLine(product.Url);
                    Console.WriteLine("-----------------------------");
                }
                Console.WriteLine("Записать в бд?(1 - если да)");
                string answer = Console.ReadLine();
                if (answer == "1")
                {
                    WriteProductsToDatabase(products);
                }
            }
            else
            {
                Console.WriteLine("Список пуст");
            }
        }

        static async Task LoadOzon(Parser parser, string url = "https://www.ozon.ru/category/smartfony-15502/")
        {
            List<Product> products = new List<Product>();
            try
            {
                products = await parser.GetLinksOzon(url);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (products.Count > 0)
            {
                Console.WriteLine($"Обнаружено {products.Count} товаров");
                foreach (var product in products)
                {
                    Console.WriteLine(product.Name);
                    Console.WriteLine(product.Price);
                    Console.WriteLine(product.Url);
                    Console.WriteLine("-----------------------------");
                }
                Console.WriteLine("Записать в бд?(1 - если да)");
                string answer = Console.ReadLine();
                if (answer == "1")
                {
                    using (var db = new Database1Entities())
                    {
                        db.Products.AddRange(products);
                        db.SaveChanges();
                        Console.WriteLine("Записано в БД");
                    }
                }
            }
        }

        static bool WriteProductsToDatabase(List<Product> products)
        {
            using (var db = new Database1Entities())
            {
                foreach (var product in products)
                {
                    var existingProduct = db.Products.FirstOrDefault(p => p.Name == product.Name);
                    if (existingProduct != null)
                    {
                        existingProduct.Description = product.Description;
                        existingProduct.Price = product.Price;
                        existingProduct.UpdatedDate = DateTime.Now;
                        existingProduct.Url = product.Url;
                    }
                    else
                    {
                        product.CreatedDate = DateTime.Now;
                        product.UpdatedDate = DateTime.Now;
                        db.Products.Add(product);
                    }
                }

                db.SaveChanges();
                Console.WriteLine("Записано в БД");
                return true;
            }
        }

        static class dbViewer
        {
            private static int _currentPage = 1;
            private static int CurrentPage
            {
                get { return _currentPage; }
                set
                {
                    if (value <= 0)
                    {
                        _currentPage = 1;
                    }
                    else _currentPage = value;
                }
            }
            private static int pageSize = 5;

            public static bool Main()
            {
                List<object> products = GetObjectFromDatabase();

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"Страница: {CurrentPage}");
                    Console.WriteLine($"{ConsoleKey.RightArrow}. Показать следующию страницу");
                    Console.WriteLine($"{ConsoleKey.LeftArrow}. Показать предыдущию страницу");
                    Console.WriteLine($"{ConsoleKey.Escape}. Выход");
                    Console.WriteLine("------------------------");
                    ShowObjects(products);
                    var input = Console.ReadKey().Key;

                    if (input == ConsoleKey.RightArrow)
                    {
                        CurrentPage++;
                    }
                    else if (input == ConsoleKey.LeftArrow)
                    {
                        CurrentPage--;
                    }
                    else if (input == ConsoleKey.Escape)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                    products = GetObjectFromDatabase();
                }
                return true;
            }

            static List<object> GetObjectFromDatabase()//как-то нужно указать из какой таблицы берется и под какую vm подстраивать
            {
                var products = new List<object>();
                var products2 = new List<object>();
                using (var db = new Database1Entities())
                {
                    products.AddRange(
                        db.Products
                        .OrderBy(p=>p.Id)
                        .Skip((CurrentPage - 1) * pageSize)
                        .Take(pageSize)
                        .ToList()
                        );
                    products2.AddRange(
                        db.Products.ToList());

                }
                return products;
            }


            static void ShowObjects(List<object> objs)
            {
                if (objs == null || !objs.Any()) { Console.WriteLine("Нет записей"); return; }

                // Получаем тип первого объекта в списке
                var modelType = objs.First().GetType();

                // Выводим заголовки свойств (с учетом DisplayNameAttribute)
                Console.WriteLine("Объекты:");
                foreach (var propertyInfo in modelType.GetProperties())
                {
                    // Проверяем, есть ли DisplayNameAttribute
                    var displayNameAttr = propertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute;
                    var displayName = displayNameAttr != null ? displayNameAttr.DisplayName : propertyInfo.Name;

                    Console.Write($"{displayName}\t");
                }
                Console.WriteLine();

                // Выводим значения каждого объекта
                foreach (var item in objs)
                {
                    foreach (var propertyInfo in modelType.GetProperties())
                    {
                        var value = propertyInfo.GetValue(item, null) ?? "null";
                        Console.Write($"{value}\t");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
