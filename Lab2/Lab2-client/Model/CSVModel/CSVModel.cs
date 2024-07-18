using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ArchitectureOfInformationSystems.MVC.Model.CSVModel
{
    public class CSVModel<T> : IModel<T> where T : class, new()
    {
        private readonly string pathFile;
        private List<T> table;

        public CSVModel(string pathFile)
        {
            if (pathFile is null || !FileManagement.CheckFile(pathFile))
                throw new FileNotFoundException();
            this.pathFile = pathFile;
            table = new();

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

        public void AddEntry(IEnumerable<string> entryFields, bool NeedSave = true)
        {
            T entry;
            try
            {
                entry = TryCreateEntry(entryFields);
                if (NeedSave)
                    UploadTable();
            }
            catch { throw; }
            table.Add(entry);
            if (NeedSave)
                SaveTable();
        }

        public void AddValues(List<T> values, bool NeedSave = true)
        {
            foreach (var value in values)
                table.Add(value);
            if (NeedSave)
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
                    properties[i].SetValue(newEntry, Validator.ConvertToType(properties[i].PropertyType, entryFields.ElementAt(i)));
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
                UploadTable();
                table.RemoveAt(key);
            }
            catch { throw; }
            SaveTable();
        }

        public void EditEntry(int key, IEnumerable<string> entryFields)
        {
            T entry;
            try
            {
                entry = TryCreateEntry(entryFields);
                UploadTable();
            }
            catch { throw; }
            table[key] = entry;
            SaveTable();
        }

        public List<T> GetValues()
        {
            try
            {
                UploadTable();
            }
            catch { throw; }
            return table;
        }

        public void OverwritingTable(List<T> values)
        {
            table.Clear();
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
