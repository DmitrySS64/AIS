using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_server.Models
{
    public static class FileReader
    {
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
    public static class FileWriter
    {
        public static void OverwriteFile(string path, List<string> lines)
        {
            using StreamWriter sw = new(path, false, System.Text.Encoding.Unicode);
            foreach (var line in lines)
            {
                sw.WriteLine(line);
            }
        }

        public static void AppendToFile(string path, string line)
        {
            using StreamWriter sw = new(path, true, System.Text.Encoding.Unicode);
            sw.WriteLine(line);
        }
    }
}
