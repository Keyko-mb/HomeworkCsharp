using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;

namespace Lab3_2_
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

            string from = "01/01/2021";
            string to = "01/02/2021";
            XDocument doc;
            string uri = "http://www.cbr.ru/scripts/xml_metall.asp?date_req1=" + from + "&date_req2=" + to;
            doc = XDocument.Load(uri);

            var records = doc.Element("Metall").Elements("Record")
                .GroupBy(record => record.Attribute("Code").Value);

            foreach (var group in records)
            {
                Series series = new Series();
                series.ChartType = SeriesChartType.Line;
                series.BorderWidth = 2;
                series.LegendText = group.Key;

                foreach (var record in group)
                {
                    series.Points.AddXY(record.Attribute("Date").Value, float.Parse(record.Element("Buy").Value));
                }

                chart.Series.Add(series);
            }

            chart.ChartAreas.Add(new ChartArea());

            Application.Run(chartForm);
        }
    }
}
