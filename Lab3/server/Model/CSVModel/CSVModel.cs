using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lab2_server.Models
{
    public class CSVModel<T> : IModel<T> where T : class, new()
    {
        private readonly string pathFile;
        private List<T> table;

        public CSVModel(string pathFile)
        {
            Console.WriteLine(pathFile);
            if (pathFile is null || !FileManagement.CheckFile(pathFile))
                throw new FileNotFoundException();
            this.pathFile = pathFile;
            table = new();
            UploadTable();
        }

        private void UploadTable()
        {
            table.Clear();

            var entries = FileManagement.GetTableStr(pathFile);
            foreach (var entry in entries)
            {
                AddEntry(entry, false);
            }
        }

        public void AddEntry(IEnumerable<string> entryFields, bool needSave = true)
        {
            T entry;
            try
            {
                entry = TryCreateEntry(entryFields);
                table.Add(entry);
                if (needSave)
                    SaveTable();
            }
            catch { throw; }
        }

        public void AddValues(List<T> values, bool needSave = true)
        {
            table.AddRange(values);
            if (needSave)
                SaveTable();
        }

        private T TryCreateEntry(IEnumerable<string> entryFields)
        {
            T newEntry = new();
            var properties = newEntry.GetType().GetProperties();
            if (entryFields.Count() != properties.Length)
                throw new Exception("Количество элементов массива не соответствует количеству свойств объекта");
            int i = 0;
            try
            {
                for (; i < entryFields.Count(); i++)
                {
                    //properties[i].SetValue(newEntry, Validator.ConvertToType(properties[i].PropertyType, entryFields.ElementAt(i)));
                    //----------------------------------------------------------------
                    var property = properties[i];
                    var value = Validator.ConvertToType(property.PropertyType, entryFields.ElementAt(i));
                    property.SetValue(newEntry, value); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Не удалось преобразовать поле {properties[i].Name} в тип {properties[i].PropertyType}\n", ex);
            }
            return newEntry;
        }

        public void RemoveEntry(int key)
        {
            try
            {
                table.RemoveAt(key);
                SaveTable();
            }
            catch { throw; }
        }

        public void EditEntry(int key, IEnumerable<string> entryFields)
        {
            T entry;
            try
            {
                entry = TryCreateEntry(entryFields);
                table[key] = entry;
                SaveTable();
            }
            catch { throw; }
            
        }

        public List<T> GetValues()
        {
            return table;
        }

        public void OverwritingTable(List<T> values)
        {
            table = new List<T>(values);
            SaveTable();
        }

        private void SaveTable()
        {
            List<string[]> list = new();
            PropertyInfo[] properties;
            if (table.Count > 0)
            {
                properties = table[0].GetType().GetProperties();
                foreach (var entry in table)
                {
                    list.Add(properties.Select(x => $"{x.GetValue(entry)}").ToArray());
                }
            }
            FileManagement.SaveTable(pathFile, list);
        }
    }
}

//var studentModel = new CSVModel<Student>("path/to/your/file.csv");
// Добавление новой записи
//studentModel.AddEntry(new string[] { "John", "Doe", "25", "true" });
// Получение всех записей
//var students = studentModel.GetValues();
// Редактирование записи
//studentModel.EditEntry(0, new string[] { "Jane", "Doe", "22", "true" });
// Удаление записи
//studentModel.RemoveEntry(0);