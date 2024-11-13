using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureOfInformationSystems.MVC.Model
{
    public static class Validator
    {
        public static object ConvertToType(Type type, string str)
        {
            try
            {
                if (type == typeof(int))
                {
                    if (int.TryParse(str, out var value))
                        return value;
                    else throw new FormatException();
                }
                else if (type == typeof(bool))
                {
                    if (bool.TryParse(str, out var value))
                        return value;
                    else throw new FormatException();
                }
                else if (type == typeof(double))
                {
                    if ((double.TryParse(str, out var value)))
                        return value;
                    else throw new FormatException();
                }
                else if (type == typeof(string))
                    return str;
                else if (type.IsEnum)
                {
                    // Если тип - это Enum, то парсим строку в соответствующее значение Enum
                    return Enum.Parse(type, str);
                }
                return Convert.ChangeType(str, type);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при конвертации данных {ex.Message}");
            }
        }
    }
}
