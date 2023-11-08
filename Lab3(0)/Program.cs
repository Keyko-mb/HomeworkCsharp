using System.Diagnostics.SymbolStore;
using System.Xml.Linq;

XDocument doc;
string uri = "C:\\Users\\shafi\\source\\repos\\Lab3\\Lab3(0)\\XMLFile1.xml";
doc = XDocument.Load(uri);

Console.WriteLine("Задание 1");
string searchName = "Петров Петр Петрович";
bool jobSortReverse = false;
int from = 2010;
int to = 2023;

var employee = doc.Element("Сотрудники").Elements("Сотрудник")
    .Where(e => e.Element("ФИО").Value.Equals(searchName))
    .Select(e => new
    {
        name = e.Element("ФИО").Value,

        jobs = (jobSortReverse ?
        e.Element("Список_Работ")
        .Elements("Работа")
        .OrderBy(work => work.Element("Дата_начала").Value.Split("-")[0])
        .Select(work => work)
        : e.Element("Список_Работ")
        .Elements("Работа")
        .OrderByDescending(work => work.Element("Дата_начала").Value.Split("-")[0])
        .Select(work => work)),
        
        payments = e.Element("Список_Зарплат")
        .Elements("Зарплата")
        .Where(payment => (Int32.Parse(payment.Element("Год").Value) > from) && (Int32.Parse(payment.Element("Год").Value)) < to)
        .Select(payment => payment),
    }) ;

foreach (var el in employee)
{
    Console.WriteLine(el.name);
    foreach (var job in el.jobs) Console.WriteLine(job);
    foreach (var payment in el.payments) Console.WriteLine(payment);
}

Console.WriteLine("\nЗадание 2");
var departments = doc.Descendants("Работа")
            .GroupBy(r => (string)r.Element("Отдел"))
            .Select(group => new
            {
                DepartmentName = group.Key,
                TotalEmployeeCount = group.Count(),
                JobTitles = group.Select(r => (string)r.Element("Название")).Distinct().ToList(),
                RealEmployeesCount = group.Count(r => r.Element("Дата_окончания").Value == "")
            })
            .ToList();

foreach (var department in departments)
{
    double departmentShare = (double)department.RealEmployeesCount / department.TotalEmployeeCount * 100;
    Console.WriteLine($"Отдел: {department.DepartmentName}");
    Console.WriteLine($"Количество работающих сотрудников: {department.RealEmployeesCount}");
    Console.WriteLine("Список должностей:");
    foreach (var jobTitle in department.JobTitles)
    {
        Console.WriteLine(jobTitle);
    }
    Console.WriteLine($"Доля работающих сотрудников от общего числа сотрудников отдела: {departmentShare}%\n");
}
