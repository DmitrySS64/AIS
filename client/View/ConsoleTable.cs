namespace client.View
{
    public class ConsoleTable
    {
        private readonly string[] _headers;
        private readonly List<string[]> _rows = new List<string[]>();

        public ConsoleTable(params string[] headers)
        {
            _headers = headers;
        }

        public void AddRow(params object[] values)
        {
            if (values.Length != _headers.Length)
            {
                throw new ArgumentException("Количество значений в строке должно соответствовать количеству заголовков.");
            }
            _rows.Add(values.Select(v => v?.ToString() ?? string.Empty).ToArray());
        }

        public void Write()
        {
            int[] columnWidths = _headers.Select((header, index) =>
                Math.Max(_rows.Max(row => row[index]?.Length ?? 0), header.Length)).ToArray();

            string separator = new string('-', columnWidths.Sum() + columnWidths.Length * 3 + 1);

            Console.WriteLine(separator);
            Console.WriteLine("| " + string.Join(" | ", _headers.Select((header, index) => header.PadRight(columnWidths[index]))) + " |");
            Console.WriteLine(separator);

            foreach (var row in _rows)
            {
                Console.WriteLine("| " + string.Join(" | ", row.Select((value, index) => value.PadRight(columnWidths[index]))) + " |");
            }

            Console.WriteLine(separator);
        }

        public static void PrintTable(List<Dictionary<string, object>> objects)
        {
            var table = new ConsoleTable(objects.FirstOrDefault()?.Keys.ToArray());
            foreach (var obj in objects)
            {
                table.AddRow(obj.Values.ToArray());
            }
            table.Write();
        }
        public static void PrintTableRecord(Dictionary<string, object> obj)
        {
            var table = new ConsoleTable(obj.Keys.ToArray());
            table.AddRow(obj.Values.ToArray());
            table.Write();
        }
    }

}
