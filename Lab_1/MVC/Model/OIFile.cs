using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureOfInformationSystems.MVC.Model
{
    /// <summary>
    /// Класс для чтения файла
    /// </summary>
    public static class FileReader
    {
        /// <summary>
        /// Прочитать весь файл
        /// </summary>
        /// <param name="path">Расположение файла</param>
        /// <returns>Список записей</returns>
        public static List<string> ReadLines(string path)
        {
            using StreamReader sr = new(path, System.Text.Encoding.Default);
            List<string> lines = new();
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }
            return lines;
        }
    }
    /// <summary>
    /// Класс для записи в файл
    /// </summary>
    public static class FileWriter
    {
        /// <summary>
        /// Перезапись файла
        /// </summary>
        /// <param name="path">Расположение файла</param>
        /// <param name="lines">Новые записи</param>
        public static void OverwriteFile(string path, List<string> lines)
        {
            using StreamWriter sw = new(path, false, System.Text.Encoding.Unicode);
            foreach (var line in lines)
            {
                sw.WriteLine(line);
            }
        }

        /// <summary>
        /// Добавить запись в файл
        /// </summary>
        /// <param name="path">Расположение файла</param>
        /// <param name="line">Новая запись</param>
        public static void AppendToFile(string path, string line)
        {
            using StreamWriter sw = new(path, true, System.Text.Encoding.Unicode);
            sw.WriteLine(line);
        }
    }
}
