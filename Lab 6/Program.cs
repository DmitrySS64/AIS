using AngleSharp;
using AngleSharp.Browser;
using AngleSharp.Dom;
using AngleSharp.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            //await LoadOzon(parser);
            
            Console.WriteLine("Конец..........");
            Console.ReadKey();
        }

        static async Task LoadOzon(Parser parser)
        {
            List<Product> products = new List<Product>();
            try
            {
                products = await parser.GetLinksOzon("https://www.ozon.ru/category/smartfony-15502/");

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


    }
}
