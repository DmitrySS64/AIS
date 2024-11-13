using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;


namespace Lab_9
{
    internal class COMFormatterExcel
    {
        Excel.Application excelApp;
        Excel.Workbook workbook;
        Excel.Worksheet worksheet;
        private string outputPath = @"D:\мусор\Output.xlsx";

        public COMFormatterExcel()
        {
            excelApp = new Excel.Application();
            OpenApp();

            workbook = excelApp.Workbooks.Add();
            worksheet = (Excel.Worksheet)workbook.Worksheets.get_Item(1);
        }

        public void FillingInTheCells()
        {
            int number = 60;
            for (int j = 1; j <= 10; j++) {
                worksheet.Cells[1, j] = j;
                worksheet.Cells[2, j] = number;
                number--;
            }
        }

        public void UseForm()
        {
            Excel.Range rng = worksheet.Range["A3"];
            rng.Formula = "=SUM(A1:J1)";
            rng.FormulaHidden = false;

            Excel.Borders border = rng.Borders;
            border.LineStyle = Excel.XlLineStyle.xlContinuous;
        }

        public void CreateDiagramm()
        {
            Excel.ChartObjects chartObjs = (Excel.ChartObjects)worksheet.ChartObjects();
            Excel.ChartObject chartObj = chartObjs.Add(10, 50, 500, 300);
            Excel.Chart xlChart = chartObj.Chart;
            Excel.Range rng2 = worksheet.Range["A1:J1"];
            Excel.Range rng3 = worksheet.Range["A3:J1"];

            xlChart.ChartType = Excel.XlChartType.xlXYScatterSmooth;

            SeriesCollection seriesCollection = (SeriesCollection)xlChart.SeriesCollection(Type.Missing);
            Series series = seriesCollection.NewSeries();
            series.XValues = worksheet.get_Range("A1", "J1");
            series.Values = worksheet.get_Range("A2", "J2");

            xlChart.HasTitle = true;
            xlChart.ChartTitle.Text = "График зависимости";

            xlChart.HasLegend = true;
            series.Name = "График";
        }

        public void OpenApp()
        {
            excelApp.Visible = true;
            excelApp.UserControl = true;
        }

        public void TrySave()
        {
            try
            {
                workbook.SaveAs(outputPath, Excel.XlFileFormat.xlOpenXMLWorkbook);//как сохранять?
                Console.WriteLine("Файл успешно сохранен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении файла: " + ex.Message);
            }
        }

        public void Close()
        {
            try
            {
                workbook.Save();
                workbook.Close(false); // Закрытие без сохранения изменений (если сохранено)
                excelApp.Quit(); // Выход из Excel
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при закрытии: " + ex.Message);
            }
        }
    }
}
