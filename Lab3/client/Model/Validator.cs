using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Model
{
    public static class Validator
    {
        public static object ConvertToType(Type type, string str)
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
            else throw new Exception("Тип данных не поддерживается");
        }
    }
}
