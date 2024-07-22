using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Models
{
    public static class FileReader
    {
        public static List<string> ReadLines(string path)
        {
            List<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }
    }
    public static class FileWriter
    {
        public static void OverwriteFile(string path, List<string> lines)
        {
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Unicode))
            {
                foreach (var line in lines)
                {
                    sw.WriteLine(line);
                }
            }
        }

        public static void AppendToFile(string path, string line)
        {
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Unicode)) {
                sw.WriteLine(line);
            }
        }
    }
}
