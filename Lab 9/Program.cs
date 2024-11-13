using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using word = Microsoft.Office.Interop.Word;
using excel = Microsoft.Office.Interop.Excel;

namespace Lab_9
{
    internal class Program
    {
        //Имя
        static string[] Names =
        {
            "Дима",
            "Семен",
            "Ваня"
        };
        //Это
        static string[] Is =
        {
            "странный",
            "немного подозрительный",
            "слишком умный",
            "загадочный",
            "сверхъестественный",
            "просто другой",
            "эксцентричный"
        };

        static string[] Relationships =
        {
            "близкий друг",
            "дальний родственник",
            "коллега по спорту",
            "тайный поклонник",
            "старый знакомый",
            "одноклассник",
            "забавный сосед",
            "тайный агент",
            "заклятый враг",
            "забытый брат",
            "верный помощник",
            "тренер по йоге",
            "покровитель",
            "хранитель"
        };

        static string[] ToWhom =
        {
            "кота",
            "кактуса",
            "своего холодильника",
            "загадочного жука",
            "чайного грибка",
            "автомата с игрушками",
            "подозрительного манекена",
            "кофеварки",
            "деревянного глобуса",
            "пальмы на балконе",
            "игровой приставки",
            "кухни своей бабушки",
            "соседа по лестничной клетке",
            "личного холодильника",
            "своего детского плюшевого медведя"
        };

        static string[] Does =
        {
            "разговаривает",
            "спорит",
            "танцует",
            "поёт серенады",
            "устраивает лекции",
            "читает стихи",
            "обучает кунг-фу",
            "делится секретами"
        };

        static string[] WithIt =
        {
            "по пятницам",
            "каждое утро",
            "на закате",
            "перед сном",
            "в особых случаях",
            "во время дождя",
            "сразу после работы",
            "по праздникам"
        };

        static void Main(string[] args)
        {
            Excel();
        }

        static void Word()
        {
            Random random = new Random();

            COMFormatter comFormatter = new COMFormatter(@"D:\мусор\Temp.doc");

            string name = Names[random.Next(Names.Length)];
            string isSomething = Is[random.Next(Is.Length)];
            string relationship = Relationships[random.Next(Relationships.Length)];
            string toWhom = ToWhom[random.Next(ToWhom.Length)];
            string does = Does[random.Next(Does.Length)];
            string withIt = WithIt[random.Next(WithIt.Length)];


            //string phrase = $"Это {name}. И он – {isSomething}. Знаете почему? Все, потому что он {relationship} {toWhom}. А ещё, он {does} {withIt}.";
            //Console.WriteLine(phrase);
            //Console.ReadLine();
            comFormatter.Replace("{имя}", name);
            comFormatter.Replace("{это}", isSomething);
            comFormatter.Replace("{кто-то}", relationship);
            comFormatter.Replace("{кому-то}", toWhom);
            comFormatter.Replace("{делает}", does);
            comFormatter.Replace("{с этим}", withIt);


            comFormatter.ReplaceBookmark("mark", "Это было закладкой");
            comFormatter.ReplaceBookmark("ne_mark", "Это не было закладкой (было)");
            comFormatter.Close();
        }

        static void Excel()
        {
            COMFormatterExcel comFormatterExcel = new COMFormatterExcel();

            // Заполнение ячеек
            comFormatterExcel.FillingInTheCells();

            // Применение формул
            comFormatterExcel.UseForm();

            // Создание диаграммы
            comFormatterExcel.CreateDiagramm();

            // Попытка сохранить файл
            comFormatterExcel.TrySave();

            // Закрытие Excel
            comFormatterExcel.Close();

            Console.WriteLine("Работа завершена.");
        }
    }
}
