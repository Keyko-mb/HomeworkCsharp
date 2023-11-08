using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;

namespace Lab3_1_
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form chartForm = new Form();
            Chart chart = new Chart();
            chart.Parent = chartForm;
            chart.Dock = DockStyle.Fill;

            Series series = new Series();
            series.ChartType = SeriesChartType.Line;
            series.BorderWidth = 2;

            string code = "R01235";
            string from = "02/01/2010";
            string to = "20/02/2022";
            XDocument doc;
            string uri = "http://www.cbr.ru/scripts/XML_dynamic.asp?date_req1=" + from + "&date_req2= " + to + "&VAL_NM_RQ=" + code;
            doc = XDocument.Load(uri);

            var records = doc.Element("ValCurs").Elements("Record");

            var count = records.Count();

            var points = records.Select(e => new
            {
                x = e.Attribute("Date").Value,
                y = e.Element("Value").Value
            }).ToList();

            foreach (var point in points)
            {
                series.Points.AddXY(point.x, float.Parse(point.y));
            }
            chart.Series.Add(series);

            chart.ChartAreas.Add(new ChartArea());

            Application.Run(chartForm);
        }
    }
}
