using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SoapServer
{
    /// <summary>
    /// Сводное описание для TempConvertService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class TempConvertService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Привет всем!";
        }
        [WebMethod]
        public string FahrenheitToCelsius(string value)
        {
            if (double.TryParse(value, out double fahrenheit))
            {
                double celsius = (fahrenheit - 32) * 5 / 9;
                return celsius.ToString("F2"); // Округляем до 2 знаков после запятой
            }
            else
            {
                return "Ошибка: введите корректное числовое значение.";
            }
        }
        [WebMethod]
        public string CelsiusToFahrenheit(string value)
        {
            if (double.TryParse(value, out double celsius))
            {
                double fahrenheit = celsius * 9 / 5 + 32;
                return fahrenheit.ToString("F2"); // Округляем до 2 знаков после запятой
            }
            else
            {
                return "Ошибка: введите корректное числовое значение.";
            }
        }
    }
}
