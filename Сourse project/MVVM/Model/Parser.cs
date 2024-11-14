using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static Сourse_project.MVVM.Model.DNSParseContext;
using static Сourse_project.MVVM.Model.CategoryService;
using System.Data.Entity.Core.Metadata.Edm;
using static Сourse_project.MVVM.Model.CasingMaterial;

namespace Сourse_project.MVVM.Model
{
    internal class Parser
    {
        string catalogUrl = "https://www.dns-shop.ru/catalog/";
        private ChromeOptions options;
        public Parser()
        {
            options = new ChromeOptions();
            options.AddArgument("--headless"); // Запуск без графического интерфейса
            options.AddArgument("--no-sandbox");
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        }

        private async Task<IWebDriver> CreateDriverAsync()
        {
            return await Task.Run(() => new ChromeDriver(options));
        }


        // Парсинг каталога
        public async Task<List<string>> ParseCatalog(string url = "https://2droida.ru/catalog/smartfony/smartfony-xiaomi?page=", int pageCount = 20, int maxWaitTimeSeconds = 10)
        {
            List<string> phoneLinks = new List<string>();
            string ProductCardSelector = ".product-item.card.h-100";
            try
            {
                using (var driver = await CreateDriverAsync())
                {
                    for (int page = 1; page <= pageCount; page++)
                    {
                        driver.Navigate().GoToUrl($"{url}{page}");

                        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                        bool isItemsFound = false;
                        DateTime startWaitTime = DateTime.Now;

                        while ((DateTime.Now - startWaitTime).TotalSeconds < maxWaitTimeSeconds)
                        {
                            var productsHtml = driver.FindElements(By.CssSelector(ProductCardSelector));
                            if (productsHtml.Count > 0)
                            {
                                isItemsFound = true;
                                break;
                            }
                            await Task.Delay(1000);
                        }
                        if (!isItemsFound)
                        {
                            Console.WriteLine($"Время ожидания превышено. \nТоваров на странице {page} найдено не было.\nПрекращаем парсинг.");
                            break;
                        }

                        var products = driver.FindElements(By.CssSelector(ProductCardSelector));
                        foreach (var element in products)
                        {
                            try
                            {
                                // Получаем ссылку на продукт
                                string link = element.FindElement(By.CssSelector("[itemprop='name'] a"))?.GetAttribute("href") ?? "";
                                if (!string.IsNullOrEmpty(link))
                                {
                                    phoneLinks.Add(link);
                                }
                            }
                            catch (Exception innerEx) { Console.WriteLine($"Ошибка при извлечении данных с элемента на странице {page}: {innerEx.Message}"); }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при парсинге каталога: {ex.Message}");
                throw;
            }
            
            return phoneLinks;
        }

        public async Task<Smartphone> ParcePhone(string url = "https://2droida.ru/product/xiaomi-redmi-12-4128gb-sky-blue-siniy-ru")
        {
            Smartphone smartphone = new Smartphone
            {
                Model = new Model
                {
                    Line = new Line(),
                    TypeOfMatrix = new TypeOfMatrix(),
                    Processor = new Processor(),
                    OS = new OS(),
                    ScreenResolution = new ScreenResolution(),
                    CasingMaterial = new CasingMaterial()
                },
                RAMSize = new RAM(),
                ColorType = new ColorType(),
                VersionType = new VersionType(),
                BuiltInMemory = new BuiltInMemory()
            };

            string ProductCardSelector = ".product-item.card.h-100";
            string buttonSelector = "button.btn.p-0.icon-link.text-primary";
            try
            {
                using (var driver = await CreateDriverAsync())
                {
                    driver.Navigate().GoToUrl(url);

                    driver.FindElement(By.CssSelector(buttonSelector)).Click(); //раскрыть подробности

                    await Task.Delay(1000);

                    var tableRows = driver.FindElements(By.CssSelector("table tbody tr"));

                    foreach (var row in tableRows)
                    {
                        var key = row.FindElement(By.CssSelector("th")).Text.Trim();
                        var value = row.FindElement(By.CssSelector("td p")).Text.Trim();

                        switch (key)
                        {
                            case "Бренд":
                                smartphone.Model.Line.BrandId = await GetOrCreateBrandIdAsync(value);
                                break;
                            case "Модель":
                                smartphone.Model.Name = value;
                                break;
                            case "Линейка":
                                smartphone.Model.Line.Name = value;
                                break;
                            case "Гарантия":
                                smartphone.Warranty = value;
                                break;
                            case "Срок службы":
                                smartphone.Model.Lifespan = value;
                                break;
                            case "Цвет":
                                smartphone.ColorType.Name = value;
                                break;
                            case "Версия":
                                smartphone.VersionType.Name = value;
                                break;
                            case "Тип матрицы экрана":
                                smartphone.Model.TypeOfMatrix.TypeName = value;
                                break;
                            case "Диагональ экрана":
                                smartphone.Model.ScreenSize = value;
                                break;
                            case "Разрешение Экрана":
                                smartphone.Model.ScreenResolution.Resolution = value;
                                break;
                            case "Оперативная память":
                                smartphone.RAMSize.Size = value;
                                break;
                            case "Встроенная память":
                                smartphone.BuiltInMemory.Name = value;
                                break;
                            case "Процессор":
                                smartphone.Model.Processor.Name = value;
                                break;
                            case "Операционная система":
                                smartphone.Model.OS.Type = value;
                                break;
                            case "Количество SIM-карт":
                                smartphone.Model.SIMCount = int.Parse(value);
                                break;
                            case "Емкость аккумулятора":
                                smartphone.Model.BatteryCapacity = value;
                                break;
                            case "Время заряда":
                                smartphone.Model.ChargingTime = value;
                                break;
                            case "Разъём для наушников":
                                smartphone.Model.HasHeadphoneJack = "mini jack 3.5 mm";
                                break;
                            case "Дата анонсирования":
                                smartphone.Model.ReleaseDate = value;
                                break;
                            case "Материал корпуса":
                                smartphone.Model.CasingMaterial.Name = value;
                                break;
                            case "Вес, г":
                                smartphone.Model.Weight = decimal.Parse(value);
                                break;
                            case "Особенности":
                                smartphone.Model.Features = value;
                                break;
                            case "Полное название товара":
                                smartphone.Title = value;
                                break;
                            default:
                                break;
                        }
                    }
                    smartphone.Model.LineId = await GetOrCreateLineIdAsync(smartphone.Model.Line.Name, smartphone.Model.Line.BrandId);
                    smartphone.ModelId = await GetOrCreateModelIdAsync(smartphone.Model.Name, smartphone.Model.LineId);
                    smartphone.ColorTypeId = await GetOrCreateColorTypeIdAsync(smartphone.ColorType.Name);
                    smartphone.RAMSizeId = await GetOrCreateRAMSizeIdAsync(smartphone.RAMSize.Size);
                    smartphone.BuiltInMemoryId = await GetOrCreateBuiltInMemoryIdAsync(smartphone.BuiltInMemory.Name);
                }
                using (var context = new ParseProductsContext())
                {
                    context.Smartphones.Add(smartphone);
                    await context.SaveChangesAsync();
                }
            }
            
            catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при парсинге каталога: {ex.Message}");
                    throw;
                }

            return smartphone;
        }


        //public Task<List<Catalog>> ParseCatalog()
        //{
        //    string categorySelector = ".subcategory__item.subcategory__item_with-childs";
        //    string listClassSelector = "subcategory__childs-list";

        //    var options = new ChromeOptions();
        //    options.AddArgument("--headless"); // Запуск без графического интерфейса
        //    options.AddArgument("--no-sandbox");
        //    options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        //    var categories = new List<Catalog>();

        //    try
        //    {
        //        using (IWebDriver driver = new ChromeDriver(options))
        //        {
        //            // Переход на указанную страницу
        //            driver.Navigate().GoToUrl(catalogUrl);

        //            // Ожидание загрузки элементов с классом "product-item card"
        //            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //            wait.Until(drv => drv.FindElements(By.CssSelector(categorySelector)).Count > 0);

        //            // Получаем все элементы с классом "product-item card"
        //            var categoriesHtml = driver.FindElements(By.CssSelector(categorySelector));


        //            foreach (var element in categoriesHtml)
        //            {
        //                var categoryLinkEl = element.FindElement(By.CssSelector($".{listClassSelector} li a"));
        //                string catagoryName = categoryLinkEl.Text; // >Бытовая техника<
        //                string categoryUrl = categoryLinkEl.GetAttribute("href");

        //                var category = new Catalog
        //                {
        //                    Name = catagoryName,
        //                    Url = categoryUrl
        //                };
        //                categories.Add(category);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Ошибка при парсинге каталога: {ex.Message}");
        //    }

        //    return Task.FromResult(categories);
        //}


        //public Task<List<Subsection>> ParseSection(Catalog catalog)
        //{
        //    string categorySelector = ".subcategory__item.ui-link";

        //    var options = new ChromeOptions();
        //    options.AddArgument("--headless"); // Запуск без графического интерфейса
        //    options.AddArgument("--no-sandbox");
        //    options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

        //    var categories = new List<Subsection>();

        //    try
        //    {
        //        using (IWebDriver driver = new ChromeDriver(options))
        //        {
        //            // Переход на указанную страницу
        //            driver.Navigate().GoToUrl(catalog.Url);

        //            // Ожидание загрузки элементов с классом "product-item card"
        //            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //            //wait.Until(drv => drv.FindElements(By.CssSelector(categorySelector)).Count > 0);

        //            // Получаем все элементы с классом "product-item card"
        //            var categoriesHtml = driver.FindElements(By.CssSelector(categorySelector));


        //            foreach (var element in categoriesHtml)
        //            {
        //                string catagoryName = element.FindElement(By.CssSelector("span")).Text;
        //                string categoryUrl = element.GetAttribute("href");

        //                var category = new Subsection
        //                {
        //                    Name = catagoryName,
        //                    Url = categoryUrl,
        //                    CatalogId = catalog.Id
        //                };
        //                categories.Add(category);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Ошибка при парсинге {catalog.Name}: {ex.Message}");
        //    }

        //    return Task.FromResult(categories);
        //}

        //public async Task<List<SubsectionOrProduct>> ParseSubsection(Subsection subsection) //возвращает либо продукты либо подкатегории
        //{
        //    var result = new List<SubsectionOrProduct>();
        //    string categorySelector = ".subcategory__item.ui-link"; // Подкатегории
        //    string productSelector = "[data-id='product']"; // Продукты
        //    string priceSelector = ".product-buy__price";

        //    try
        //    {
        //        using (var driver = await CreateDriverAsync())
        //        {
        //            // Переход на указанную страницу
        //            driver.Navigate().GoToUrl(subsection.Url);

        //            // Ожидание загрузки элементов
        //            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //            wait.Until(drv => drv.FindElements(By.CssSelector(categorySelector)).Count > 0 || drv.FindElements(By.CssSelector(priceSelector)).Count > 0);

        //            // Проверяем наличие подкатегорий на странице
        //            var subsectionsHtml = driver.FindElements(By.CssSelector(categorySelector));
        //            if (subsectionsHtml.Count > 0)
        //            {
        //                // Если есть подкатегории, добавляем их в результат
        //                foreach (var element in subsectionsHtml)
        //                {
        //                    string sectionName = element.FindElement(By.CssSelector("span")).Text;
        //                    string sectionUrl = element.GetAttribute("href");

        //                    var subcategory = new Subsection
        //                    {
        //                        Name = sectionName,
        //                        Url = sectionUrl,
        //                        CatalogId = subsection.CatalogId,
        //                        ParentSubsectionId = subsection.Id
        //                    };

        //                    result.Add(new SubsectionOrProduct { Subsection = subcategory });
        //                }
        //            }
        //            else
        //            {
        //                // Если подкатегорий нет, ищем продукты
        //                string nameSelector = ".catalog-product__name";
        //                string ratingSelector = ".catalog-product__rating";

        //                var productsHtml = driver.FindElements(By.CssSelector(productSelector));

        //                foreach (var element in productsHtml)
        //                {
        //                    var productA = element.FindElement(By.CssSelector(nameSelector));
        //                    var productUrl = productA.GetAttribute("href");
        //                    var productTitle = productA.FindElement(By.CssSelector("span")).Text;

        //                    var productPriceText = element.FindElement(By.CssSelector(priceSelector)).Text; // 6 999 &nbsp ₽
        //                    int productPrice = int.TryParse(productPriceText.Replace(" ", "").Replace("₽", ""), out int price) ? price : 0;

        //                    var productRatingHtml = element.FindElement(By.CssSelector(ratingSelector));
        //                    var productRatingText = productRatingHtml.GetAttribute("data-rating");
        //                    int productRating = int.TryParse(productRatingText, out int rating) ? rating : 0;

        //                    var productReviewsCountText = productRatingHtml.FindElement(By.Id("text")).Text;
        //                    int productReviewsCount = int.TryParse(productReviewsCountText.Replace("\"", ""), out int reviewsCount) ? reviewsCount : 0;

        //                    // Разделяем productTitle на название и спецификации
        //                    string title = "";
        //                    string specs = "";

        //                    // Проверяем, если в productTitle есть квадратные скобки
        //                    int specsIndex = productTitle.IndexOf('[');
        //                    if (specsIndex > -1)
        //                    {
        //                        title = productTitle.Substring(0, specsIndex).Trim(); // Извлекаем название
        //                        specs = productTitle.Substring(specsIndex + 1, productTitle.Length - specsIndex - 2).Trim(); // Извлекаем спецификации без квадратных скобок
        //                    }
        //                    else
        //                    {
        //                        title = productTitle; // Если нет спецификаций, то полное название
        //                    }

        //                    var product = new Product
        //                    {
        //                        Title = title,
        //                        Specs = specs,
        //                        Url = productUrl,
        //                        Price = price,
        //                        Rating = rating,
        //                        ReviewsCount = productReviewsCount,
        //                        SubsectionId = subsection.Id,
        //                    };

        //                    result.Add(new SubsectionOrProduct { Product = product });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Ошибка при парсинге {subsection.Name}: {ex.Message}");
        //    }

        //    return result;
        //}

        //public async Task<List<Product>> ParseProductsListAsync(string url) //если просят с конкретной старницы
        //{
        //    var products = new List<Product>();
        //    string productSelector = "[data-id='product']";
        //    string priceSelector = ".product-buy__price";
        //    string nameSelector = ".catalog-product__name";
        //    string ratingSelector = ".catalog-product__rating";

        //    try
        //    {
        //        using (IWebDriver driver = await CreateDriverAsync())
        //        {
        //            // Переход на указанную страницу
        //            driver.Navigate().GoToUrl(url);

        //            // Ожидание загрузки элементов с классом "product-item card"
        //            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        //            wait.Until(drv => drv.FindElements(By.CssSelector(priceSelector)).Count > 0);

        //            // Получаем все элементы с классом "product-item card"
        //            var productsHtml = driver.FindElements(By.CssSelector(productSelector));


        //            foreach (var element in productsHtml)
        //            {
        //                var productA = element.FindElement(By.CssSelector(nameSelector));
        //                var productUrl = productA.GetAttribute("href");
        //                var productTitle = productA.FindElement(By.CssSelector("span")).Text;

        //                var productPriceText = element.FindElement(By.CssSelector(priceSelector)).Text; // 6 999 &nbsp ₽
        //                int productPrice = int.TryParse(productPriceText.Replace(" ","").Replace("₽", ""), out int price) ? price : 0;

        //                var productRatingHtml = element.FindElement(By.CssSelector(ratingSelector));
        //                var productRatingText = productRatingHtml.GetAttribute("data-rating");
        //                int productRating = int.TryParse (productRatingText, out int rating) ? rating : 0;

        //                var productReviewsCountText = productRatingHtml.FindElement(By.Id("text")).Text;
        //                int productReviewsCount = int.TryParse(productReviewsCountText.Replace("\"", ""), out int reviewsCount) ? reviewsCount : 0;

        //                // Разделяем productTitle на название и спецификации
        //                string title = "";
        //                string specs = "";

        //                // Проверяем, если в productTitle есть квадратные скобки
        //                int specsIndex = productTitle.IndexOf('[');
        //                if (specsIndex > -1)
        //                {
        //                    title = productTitle.Substring(0, specsIndex).Trim(); // Извлекаем название
        //                    specs = productTitle.Substring(specsIndex + 1, productTitle.Length - specsIndex - 2).Trim(); // Извлекаем спецификации без квадратных скобок
        //                }
        //                else
        //                {
        //                    title = productTitle; // Если нет спецификаций, то полное название
        //                }

        //                var product = new Product
        //                {
        //                    Title = title,
        //                    Specs = specs,
        //                    Url = productUrl,
        //                    Price = price,
        //                    Rating = rating,
        //                    ReviewsCount = productReviewsCount
        //                };
        //                products.Add(product);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Ошибка при парсинге {url}: {ex.Message}");
        //    }

        //    return products;
        //}

        //public class SubsectionOrProduct
        //{
        //    public Subsection Subsection { get; set; }
        //    public Product Product { get; set; }
        //}
    }
}
