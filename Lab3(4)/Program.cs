using System.Xml.Linq;

XDocument doc;
string uri = "C:\\Users\\shafi\\source\\repos\\Lab3\\Lab3(4)\\data-20210603T1050-structure-20150929T0000.xml";
doc = XDocument.Load(uri);

var records = doc.Element("data")
    .Elements("record")
    .Where(record => record.Element("unit").Value.Equals("Астраханская область"))
    .Select(record => record);

foreach (var record in records) Console.WriteLine(record);
