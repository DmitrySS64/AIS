using AngleSharp;
using AngleSharp.Common;
using AngleSharp.Dom;
using AngleSharp.Io;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Lab_6
{
    class Parser
    {
        private const string url = "https://www.avito.ru/";
        //avito
        public async Task ParseProductInfo(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                    client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.5");

                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Ошибка при подключении к сайту. Статус-код: " + response.StatusCode);
                        return;
                    }

                    string htmlContent = await response.Content.ReadAsStringAsync();
                    if (htmlContent.Contains("Access Denied") || htmlContent.Contains("captcha"))
                    {
                        Console.WriteLine("Доступ заблокирован файрволом или требуется капча.");
                        return;
                    }
                    IDocument doc = await BrowsingContext.New(Configuration.Default.WithDefaultLoader()).OpenAsync(req=>req.Content(htmlContent));
                    Product product = new Product();
                    var name = doc.QuerySelector("h1[itemprop='name']")?.TextContent ?? "none";
                    var price = doc.QuerySelector("span[itemprop='price']")?.GetAttribute("content") ?? "none";
                    var description = doc.QuerySelector("div[itemprop='description']")?.TextContent ?? "none";

                    product.Name = name;
                    product.Price = price;
                    product.Description = description;
                    Console.WriteLine(product.Name);
                    Console.WriteLine(product.Price);
                    Console.WriteLine(product.Description);
                    Console.WriteLine("---------------");

                    //Console.WriteLine(product.GetInfo());

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при парсинге страницы {url}: {ex.Message}");
            }
        }

        //public async Task ParseProductInfo(string url)
        //{
        //    try
        //    {
        //        IDocument doc = await BrowsingContext.New(Configuration.Default.WithDefaultLoader()).OpenAsync(url);

        //        Product product = new Product();
        //        var name = doc.QuerySelector("h1[itemprop='name']")?.TextContent ?? "none";
        //        var price = doc.QuerySelector("span[itemprop='price']")?.GetAttribute("content") ?? "none";
        //        var description = doc.QuerySelector("div[itemprop='description']")?.TextContent ?? "none";

        //        product.Name = name;
        //        product.Price = price;
        //        product.Description = description;
        //        Console.WriteLine(product.GetInfo());
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Ошибка при парсинге страницы {url}: {ex.Message}");
        //    }
        //}

        //public async Task ParseProductInfoDNS(string url)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        var response = await client.GetAsync(url);
        //        string htmlContent = await response.Content.ReadAsStringAsync();

        //        IDocument doc = await BrowsingContext.New(Configuration.Default.WithDefaultLoader()).OpenAsync(req => req.Content(url));

        //        var name = doc.QuerySelector(".product-card-top__name")?.TextContent ?? "none";
        //        Console.WriteLine($"Product Name: {name}");
        //    }
        //}

        //avito
        public async Task ListProducts(string url)
        {
            IDocument doc = await BrowsingContext.New(Configuration.Default.WithDefaultLoader()).OpenAsync(url);
            var products = doc.QuerySelectorAll("[data-marker='item']");
            //List<Product> productsList = new List<Product>();
            foreach (var item in products) {
                var a = item.QuerySelector("[itemprop='url']")?.GetAttribute("href");
                var name = item.QuerySelector("[itemprop='name']")?.TextContent;



                Console.WriteLine($"{name}:{a}");
                //await ParseProductInfo(url+a);
                Console.WriteLine();
            }
        }
        //ozon
        public async Task<List<Product>> GetLinksOzon(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                    client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.5");

                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Ошибка при подключении к сайту. Статус-код: " + response.StatusCode);
                    }

                    string htmlContent = await response.Content.ReadAsStringAsync();
                    if (htmlContent.Contains("Access Denied") || htmlContent.Contains("captcha"))
                    {
                        throw new Exception("Доступ заблокирован файрволом или требуется капча.");
                    }

                    IDocument doc = await BrowsingContext.New(Configuration.Default.WithDefaultLoader()).OpenAsync(req => req.Content(htmlContent));
                    List<Product> products = new List<Product>();


                    var productsHtml = doc.QuerySelectorAll(".tile-root");
                    foreach(var element in productsHtml)
                    {
                        var name = element.QuerySelector("span.tsBody500Medium")?.TextContent ?? "none";
                        var price = element.QuerySelector("span.c3019-a1")?.TextContent ?? "none";
                        var link = element.QuerySelector("a")?.GetAttribute("href") ?? "none";
                        products.Add(new Product
                        {
                            Name = name,
                            Price = price,
                            Url = link
                        });
                    }

                    return products;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при парсинге страницы {url}: {ex.Message}");
            }
        }

        public async Task<List<Product>> GetLinksOzon2(string url)
        {
            IDocument doc = await BrowsingContext.New(Configuration.Default.WithDefaultLoader()).OpenAsync(url);
            List<Product> products = new List<Product>();
            var productsHtml = doc.QuerySelectorAll(".tile-root");
            foreach (var element in productsHtml)
            {
                var name = element.QuerySelector("span.tsBody500Medium")?.TextContent ?? "none";
                var price = element.QuerySelector("span.c3019-a1")?.TextContent ?? "none";
                var link = element.QuerySelector("a")?.GetAttribute("href") ?? "none";
                products.Add(new Product
                {
                    Name = name,
                    Price = price,
                    Url = link
                });
            }

            return products;
        }

        public Task<List<Product>> GetProductsSimple2droida(string url)
        {
            List<Product> products = new List<Product>();

            // Указываем опции для Chrome
            var options = new ChromeOptions();
            options.AddArgument("--headless"); // Запуск без графического интерфейса
            options.AddArgument("--no-sandbox");
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            try
            {
                using (IWebDriver driver = new ChromeDriver(options))
                {
                    // Переход на указанную страницу
                    driver.Navigate().GoToUrl(url);

                    // Ожидание загрузки элементов с классом "product-item card"
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(drv => drv.FindElements(By.CssSelector(".product-item.card.h-100")).Count > 0);

                    // Получаем все элементы с классом "product-item card"
                    var productsHtml = driver.FindElements(By.CssSelector(".product-item.card.h-100"));

                    foreach (var element in productsHtml)
                    {
                        // Находим название продукта
                        var nameDiv = element.FindElement(By.CssSelector("[itemprop='name'] a"));
                        string name = nameDiv?.Text ?? "none";

                        // Получаем ссылку на продукт
                        string link = nameDiv?.GetAttribute("href") ?? "";

                        // Извлекаем цену
                        string price = element.FindElement(By.CssSelector("meta[itemprop='price']"))?.GetAttribute("content") ?? "none";

                        // Добавляем продукт в список
                        products.Add(new Product
                        {
                            Name = name,
                            Price = price,
                            Url = link
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при парсинге страницы {url}: {ex.Message}");
            }

            return Task.FromResult(products);
        }
    }
}
