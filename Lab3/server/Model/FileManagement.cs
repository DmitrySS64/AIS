namespace Lab2_server.Models
{
    public static class FileManagement
    {
        public static bool CheckFile(string path)
        {
            return File.Exists(path);
        }

        public static string[][] GetTableStr(string path, string sep = ";")
        {
            var strLines = FileReader.ReadLines(path);
            if (strLines.Count == 0) return Array.Empty<string[]>();
            string[][] tableStr = new string[strLines.Count][];
            for (int i = 0; i < strLines.Count; i++)
            {
                tableStr[i] = strLines[i].Split(sep);
            }
            return tableStr;
        }

        public static void SaveTable(string path, List<string[]> table)
        {
            FileWriter.OverwriteFile(path, table.Select(x => string.Join(";", x)).ToList());
        }


        //public static void AddRow(string path, string[] data, string sep = ";")
        //{
        //    string line = string.Join(sep, data);
        //    FileWriter.AppendToFile(path, line);
        //}

        //public static void RemoveRow(string path, int rowIndex)
        //{
        //    var table = GetTableStr(path);
        //    if (rowIndex >= 0 && rowIndex < table.Length)
        //    {
        //        List<string> lines = new List<string>();
        //        foreach (var row in table)
        //        {
        //            if (Array.IndexOf(table, row) != rowIndex)
        //            {
        //                lines.Add(string.Join(";", row));
        //            }
        //        }
        //        FileWriter.OverwriteFile(path, lines);
        //    }
        //}
        
        //private string FileName;
        ////Path.Combine(Environment.CurrentDirectory, @"Data\data.CSV");
        //public FileManagement(string filePath)
        //{
        //    if(!File.Exists(filePath))
        //        throw new FileNotFoundException();
        //    FileName = filePath;
        //}

        ////Вывод всех записей на экран
        //public List<T> ReadAllRecords()
        //{
        //    List<T> records = new();

        //    using (StreamReader reader = new StreamReader(FileName))
        //    {
        //        string? line;
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            string[] parts = line.Split(';');

        //            if (parts.Length == typeof(T).GetProperties().Length)
        //            {
        //                T record = Activator.CreateInstance<T>();

        //                for (int i = 0; i < parts.Length; i++)
        //                {
        //                    PropertyInfo property = typeof(T).GetProperties()[i];
        //                    Type type = property.PropertyType;

        //                    if (type == typeof(string))
        //                    {
        //                        property.SetValue(record, parts[i]);
        //                    }
        //                    else 
        //                    {
        //                        object parsedValue = Convert.ChangeType(parts[i], type);

        //                        if (parsedValue != null)
        //                        {
        //                            property.SetValue(record, parsedValue);
        //                        }
        //                    }
        //                }

        //                records.Add(record);
        //            }
        //        }
        //    }

        //    return records;
        //}


        ////Запись списка данных в файл
        //public void WriteRecordsToFile(List<T> records)
        //{
        //    using (StreamWriter writer = new StreamWriter(FileName, true, System.Text.Encoding.Default))
        //    {
        //        foreach (T record in records)
        //        {
        //            string line = GetRecordString(record);
        //            writer.WriteLine(line);
        //        }
        //    }
        //}

        ////Составить запись по классу
        //private string GetRecordString(T record)
        //{
        //    PropertyInfo[] properties = typeof(T).GetProperties();
        //    List<string> values = new List<string>();

        //    foreach (PropertyInfo property in properties)
        //    {
        //        object value = property.GetValue(record);
        //        string stringValue = value != null ? value.ToString() : string.Empty;
        //        values.Add(stringValue);
        //    }

        //    return string.Join(";", values);
        //}

        ////Перезаписать файл
        //public void OverwriteTheFile(List<T> records)
        //{
        //    using (StreamWriter writer = new StreamWriter(FileName, false, System.Text.Encoding.Default))
        //    {
        //        foreach (T record in records)
        //        {
        //            string line = GetRecordString(record);
        //            writer.WriteLine(line);
        //        }
        //    }
        //}
        
    }
}
