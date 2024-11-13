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
        // путь к файлу
        private readonly string pathFile;
        private List<T> table;

        public CSVModel(string pathFile)
        {
            //проверка на наличие файла
            if (pathFile is null || !FileManagement.CheckFile(pathFile))
                throw new FileNotFoundException();
            this.pathFile = pathFile;
            table = new();
        }

        /// <summary>
        /// Обновляет список из файла
        /// </summary>
        private void UploadTable()
        {
            table.Clear();

            var entries = FileManagement.GetTableStr(pathFile);
            foreach (var entry in entries)
            {
                AddEntry(entry, false);
            }
        }

        /// <summary>
        /// Adds a new entry
        /// </summary>
        /// <param name="entryFields">Коллекция полей для записи</param>
        /// <param name="NeedSave">Если стоит true сохраняет запись</param>
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

        /// <summary>
        /// Добавление записей в список
        /// </summary>
        /// <param name="values">Записи</param>
        /// <param name="NeedSave">Сохраняет файл если true</param>
        public void AddValues(List<T> values, bool NeedSave = true)
        {
            foreach (var value in values)
                table.Add(value);
            if (NeedSave)
                SaveTable();
        }

        /// <summary>
        /// Перевод колекции значений и сущность
        /// </summary>
        /// <param name="entryFields">значения записи</param>
        /// <returns>объявленная сущность</returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Удаляет запись из списка
        /// </summary>
        /// <param name="key">id записи</param>
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

        /// <summary>
        /// Заменяет одну запись на другую
        /// </summary>
        /// <param name="key">id записи для замены</param>
        /// <param name="entryFields">новая запись</param>
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

        /// <summary>
        /// Возвращает актуальные данные файла
        /// </summary>
        /// <returns>Записи файла</returns>
        public List<T> GetValues()
        {
            try
            {
                UploadTable();
            }
            catch { throw; }
            return table;
        }

        /// <summary>
        /// Задает новый список и сохраняет файл
        /// </summary>
        /// <param name="values">Новые записи</param>
        public void OverwritingTable(List<T> values)
        {
            table.Clear();
            table = new List<T>(values);
            SaveTable();
        }

        /// <summary>
        /// Переписывает исходный файл на текущие записи
        /// </summary>
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
