using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Lab_7
{
    internal class Program
    {

        static async Task Main()
        {
            Parser parser = new Parser();

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
                    DbViewer.Main();
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
            List<ProductCategory> categories = new List<ProductCategory>();
            try
            {
                var result = await parser.GetProductsSimple2droida(url);
                products = result.Item1;
                categories = result.Item2;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (products.Count > 0)
            {
                Console.WriteLine($"Обнаружено {products.Count} товаров и {categories.Count} категорий");
                Console.WriteLine("______Товары______");
                foreach (var product in products)
                {
                    Console.WriteLine(product.Name);
                    Console.WriteLine(product.Price);
                    Console.WriteLine(product.Url);
                    Console.WriteLine("-----------------------------");
                }
                Console.WriteLine("______Категории______");
                foreach (var category in categories)
                {
                    Console.WriteLine(category.Name);
                    Console.WriteLine(category.Url);
                    Console.WriteLine("-----------------------------");
                }
                Console.WriteLine("Записать в бд?(1 - если да)");
                string answer = Console.ReadLine();
                if (answer == "1")
                {
                    WriteProductsToDatabase(products, categories);
                }
            }
            else
            {
                Console.WriteLine("Список пуст");
            }
        }


        static bool WriteProductsToDatabase(List<Product> products, List<ProductCategory> categories)
        {
            using (var db = new ParseProductsContext())
            {
                foreach (var category in categories)
                {
                    var existingCategory = db.ProductCategories.FirstOrDefault(c=>c.Name == category.Name);
                    if (existingCategory == null)
                    {
                        db.ProductCategories.Add(category);
                    }
                    else
                    {
                        existingCategory.Url = category.Url;
                    }
                }
                db.SaveChanges();


                foreach (var product in products)
                {
                    var existingProduct = db.Products.FirstOrDefault(p => p.Name == product.Name);
                    var productCategory = db.ProductCategories.FirstOrDefault(c => c.Name == product.Category.Name);
                    if (existingProduct != null)
                    {
                        existingProduct.Description = product.Description;
                        existingProduct.Price = product.Price;
                        existingProduct.UpdatedDate = DateTime.Now;
                        existingProduct.Url = product.Url;

                        if (productCategory != null)
                        {
                            existingProduct.CategoryId = productCategory.Id;
                        }
                    }
                    else
                    {
                        product.CreatedDate = DateTime.Now;
                        product.UpdatedDate = DateTime.Now;

                        if (productCategory != null)
                        {
                            product.CategoryId = productCategory.Id;
                        }

                        db.Products.Add(product);
                    }
                }

                db.SaveChanges();
                Console.WriteLine("Записано в БД");
                return true;
            }
        }

        static class DbViewer
        {
            private static int _currentPage = 1;
            private static int CurrentPage
            {
                get { return _currentPage; }
                set
                {
                    _currentPage = value <= 0 ? 1 : value;
                }
            }
            private static readonly int pageSize = 5;
            private static bool isViewingCategories = true;
            private static int? selectedCategoryId = null;
            private static string selectedCategoryName = "";

            public static bool Main()
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(isViewingCategories
                        ? "Просмотр категорий"
                        : selectedCategoryId.HasValue
                            ? $"Просмотр товаров в категории {selectedCategoryName}"
                            : "Просмотр товаров");

                    Console.WriteLine($"Страница: {CurrentPage}");
                    Console.WriteLine($"Стрелка вправо. Показать следующию страницу");
                    Console.WriteLine($"Стрелка влево. Показать предыдущию страницу");
                    Console.WriteLine("Tab. Переключиться между категориями и товарами");
                    Console.WriteLine("Enter. Перейти в категорию (если выбрана категория)");
                    Console.WriteLine("Цифра 1-9. Открыть категорию по номеру");
                    Console.WriteLine($"Esc. Выход");
                    Console.WriteLine("------------------------");

                    var objects = GetObjectsFromDatabase();
                    ShowObjects(objects);

                    var input = Console.ReadKey().Key;

                    if (input == ConsoleKey.RightArrow)
                    {
                        CurrentPage++;
                    }
                    else if (input == ConsoleKey.LeftArrow)
                    {
                        CurrentPage--;
                    }
                    else if (input == ConsoleKey.Tab)
                    {
                        isViewingCategories = !isViewingCategories;
                        selectedCategoryId = null;
                        CurrentPage = 1;
                    }
                    else if (isViewingCategories && input >= ConsoleKey.D1 && input <= ConsoleKey.D9)
                    {
                        // Переход в категорию по нажатой цифре
                        int index = input - ConsoleKey.D1; // Преобразуем клавишу в индекс
                        if (index < objects.Count)
                        {
                            var productCategory = (objects[index] as ProductCategory);
                            selectedCategoryId = productCategory?.Id;
                            selectedCategoryName = productCategory?.Name;
                            isViewingCategories = false;
                            CurrentPage = 1;
                        }
                    }
                    else if (input == ConsoleKey.Enter && isViewingCategories && objects.Any())
                    {
                        // Если выбрана категория, установить её ID для просмотра товаров
                        selectedCategoryId = (objects.First() as ProductCategory)?.Id;
                        isViewingCategories = false; // Переключаемся на просмотр товаров
                        CurrentPage = 1;
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
                return true;
            }

            static List<object> GetObjectsFromDatabase()
            {
                using (var db = new ParseProductsContext())
                {
                    if (isViewingCategories)
                    {
                        return db.ProductCategories
                            .OrderBy(c => c.Id)
                            .Skip((CurrentPage - 1) * pageSize)
                            .Take(pageSize)
                            .ToList<object>();
                    }
                    else if (selectedCategoryId.HasValue)
                    {
                        return db.Products
                            .Where(p => p.CategoryId == selectedCategoryId)
                            .OrderBy(p => p.Id)
                            .Skip((CurrentPage - 1) * pageSize)
                            .Take(pageSize)
                            .ToList<object>();
                    }
                    else
                    {
                        return db.Products
                            .OrderBy(p => p.Id)
                            .Skip((CurrentPage - 1) * pageSize)
                            .Take(pageSize)
                            .ToList<object>();
                    }
                }
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
