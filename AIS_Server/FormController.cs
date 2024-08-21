using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AIS_Server
{
    public static class FormController
    {
        public static string GetFormDescription(string className)
        {
            if (!Server.availableClasses.TryGetValue(className, out Type classType))
            {
                return "Class not found";
            }

            var fields = classType.GetProperties()
                .Select(p => new FormField
                {
                    Name = p.Name,
                    Type = GetFieldType(p.PropertyType),
                    IsRequired = p.CustomAttributes.Any(a => a.AttributeType == typeof(RequiredAttribute)),
                    Label = p.Name,
                    DefaultValue = p.PropertyType.IsValueType ? Activator.CreateInstance(p.PropertyType) : null
                })
                .ToList();

            var formDescription = new FormDescription
            {
                ClassName = className,
                Fields = fields
            };

            return JsonConvert.SerializeObject(formDescription);
        }

        private static string GetFieldType(Type type)
        {
            if (type == typeof(int) || type == typeof(decimal) || type == typeof(float) || type == typeof(double))
                return "number";
            if (type == typeof(bool))
                return "checkbox";
            if (type == typeof(DateTime))
                return "date";
            return "text"; // Для всех остальных типов
        }
    }
    public class FormField
    {
        public string Name { get; set; }
        public string Type { get; set; } // Например, "text", "number", "date", "checkbox" и т.д.
        public bool IsRequired { get; set; }
        public string Label { get; set; }
        public object DefaultValue { get; set; } // Значение по умолчанию, если применимо
    }

    public class FormDescription
    {
        public string ClassName { get; set; }
        public List<FormField> Fields { get; set; }
    }

}
