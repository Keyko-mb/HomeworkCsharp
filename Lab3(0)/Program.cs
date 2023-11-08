
using System.Linq;
using System.Xml.Linq;

XDocument doc;
string uri = "C:\\Users\\shafi\\source\\repos\\HomeworkCsharp\\Lab3(0)\\XMLFile1.xml";
doc = XDocument.Load(uri);

Console.WriteLine("Введите: 1. Задания, 2. Редактировать XML");
int task = Int32.Parse(Console.ReadLine());
if (task == 1) Tasks.goTasks(doc);
else Console.WriteLine("Введите: 1. Добавить сотруднику место работы и зарплату " +
    "2. Изменить место работы и зарплату ");
task = Int32.Parse(Console.ReadLine());
if (task == 1)
{
    Console.WriteLine("Добавление места работы и зарплаты");
    string name = "Петров Петр Петрович";

    Console.WriteLine("Список работ и зарплат до: ");
    var info = doc.Descendants("Сотрудник")
    .Where(e => e.Element("ФИО").Value.Equals(name))
    .Select(e => new
    {
        jobs = e.Element("Список_Работ").Elements("Работа"),
        payments = e.Element("Список_Зарплат").Elements("Зарплата")

    });
    foreach (var el in info)
    {
        foreach (var job in el.jobs) Console.WriteLine(job);
        foreach (var payment in el.payments) Console.WriteLine(payment);
    }

    string jobName = "Механик";
    string dateFrom = "2023-06-15";
    string dateTo = "2023-06-16";
    string department = "Отдел по ремонту";
    var newWorkPlace = new XElement("Работа",
        new XElement("Название", jobName),
        new XElement("Дата_начала", dateFrom),
        new XElement("Дата_окончания", dateTo),
        new XElement("Отдел", department)
        );
    doc.Element("Сотрудники").Elements("Сотрудник")
        .First(e => e.Element("ФИО").Value.Equals(name))
        .Element("Список_Работ")
        .Add(newWorkPlace);
    string year = "2023";
    string month = "06";
    string size = "55555";
    var newPayment = new XElement("Зарплата",
        new XElement("Год", year),
        new XElement("Месяц", month),
        new XElement("Размер", size)
        );
    doc.Element("Сотрудники").Elements("Сотрудник")
        .First(e => e.Element("ФИО").Value.Equals(name))
        .Element("Список_Зарплат")
        .Add(newPayment);

    doc.Save(uri);

    Console.WriteLine("\nСписок работ и зарплат после: ");
    info = doc.Descendants("Сотрудник")
    .Where(e => e.Element("ФИО").Value.Equals(name))
    .Select(e => new
    {
        jobs = e.Element("Список_Работ").Elements("Работа"),
        payments = e.Element("Список_Зарплат").Elements("Зарплата")

    });
    foreach (var el in info)
    {
        foreach (var job in el.jobs) Console.WriteLine(job);
        foreach (var payment in el.payments) Console.WriteLine(payment);
    }
}
else
{
    Console.WriteLine("Редактирование места работы и зарплаты");
    string name = "Петров Петр Петрович";

    Console.WriteLine("Список работ и зарплат до: ");
    var info = doc.Descendants("Сотрудник")
    .Where(e => e.Element("ФИО").Value.Equals(name))
    .Select(e => new
    {
        jobs = e.Element("Список_Работ").Elements("Работа"),
        payments = e.Element("Список_Зарплат").Elements("Зарплата")

    });
    foreach (var el in info)
    {
        foreach (var job in el.jobs) Console.WriteLine(job);
        foreach (var payment in el.payments) Console.WriteLine(payment);
    }

    string oldJobName = "Директор";
    string newJobName = "Измененная должность";
    doc.Element("Сотрудники").Elements("Сотрудник")
        .First(e => e.Element("ФИО").Value.Equals(name))
        .Element("Список_Работ").Elements("Работа")
        .Where(work => work.Element("Название").Value.Equals(oldJobName)).First()
        .SetElementValue("Название", newJobName);
    string oldSize = "30000";
    string newSize = "11111";
    doc.Element("Сотрудники").Elements("Сотрудник")
        .First(e => e.Element("ФИО").Value.Equals(name))
        .Element("Список_Зарплат").Elements("Зарплата")
        .Where(payment => payment.Element("Размер").Value.Equals(oldSize)).First()
        .SetElementValue("Размер", newSize);

    doc.Save(uri);

    Console.WriteLine("\nСписок работ и зарплат после: ");
    info = doc.Descendants("Сотрудник")
    .Where(e => e.Element("ФИО").Value.Equals(name))
    .Select(e => new
    {
        jobs = e.Element("Список_Работ").Elements("Работа"),
        payments = e.Element("Список_Зарплат").Elements("Зарплата")

    });
    foreach (var el in info)
    {
        foreach (var job in el.jobs) Console.WriteLine(job);
        foreach (var payment in el.payments) Console.WriteLine(payment);
    }
}