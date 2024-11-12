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

namespace Lab_7
{
    class Parser
    {
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


        public Task<Tuple<List<Product>,List<ProductCategory>>> GetProductsSimple2droida(string url)
        {
            List<Product> products = new List<Product>();
            List<ProductCategory> productCategories = new List<ProductCategory>();
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
                        Product product = new Product();
                        ProductCategory productCategory = new ProductCategory();

                        // Находим название продукта
                        var nameDiv = element.FindElement(By.CssSelector("[itemprop='name'] a"));
                        string name = nameDiv?.Text ?? "none";

                        // Получаем ссылку на продукт
                        string link = nameDiv?.GetAttribute("href") ?? "";

                        // Извлекаем цену
                        string price = element.FindElement(By.CssSelector("meta[itemprop='price']"))?.GetAttribute("content") ?? "none";

                        var categoryA = element.FindElement(By.CssSelector(".product-category a"));
                        string category = categoryA?.Text ?? "";
                        string categoryUrl = categoryA.GetAttribute("href") ?? "";

                        product.Name = name;
                        product.Price = price;
                        product.Url = link;

                        if (!string.IsNullOrEmpty(category))
                        {
                            productCategory.Name = category;
                            productCategory.Url = categoryUrl;

                            foreach (var cat in productCategories)
                            {
                                if (cat.Name == category)
                                {
                                    productCategory = cat;
                                    break;
                                }
                            }
                            productCategories.Add(productCategory);
                            product.Category = productCategory;
                        }
                        products.Add(product);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при парсинге страницы {url}: {ex.Message}");
            }

            return Task.FromResult(new Tuple<List<Product>, List<ProductCategory>>(products, productCategories));
        }
    }
}
