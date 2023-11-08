using System.Xml.Linq;

class Tasks
{
    public static void goTasks(XDocument doc)
    {
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
            });
        foreach (var el in employee)
        {
            Console.WriteLine(el.name);
            foreach (var job in el.jobs) Console.WriteLine(job);
            foreach (var payment in el.payments) Console.WriteLine(payment);
        }


        Console.WriteLine("\nЗадание 2");
        var departments = doc.Descendants("Работа")
                    .GroupBy(work => (string)work.Element("Отдел"))
                    .Select(group => new
                    {
                        DepartmentName = group.Key,
                        TotalEmployeeCount = group.Count(),
                        JobTitles = group.Select(work => (string)work.Element("Название")).Distinct().ToList(),
                        RealEmployeesCount = group.Count(work => work.Element("Дата_окончания").Value == "")
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


        Console.WriteLine("\nЗадание 3");
        var employees = doc.Element("Сотрудники").Elements("Сотрудник")
                    .Where(e => e.Elements("Список_Работ").Elements("Работа")
                        .Count(work => work.Element("Дата_окончания").Value == "") > 1)
                    .GroupBy(e => e.Element("ФИО").Value)
                    .Select(g => new
                    {
                        name = g.Key,
                        departments = g.SelectMany(e => e.Descendants("Работа")
                        .Where(work => work.Element("Дата_окончания").Value == "")
                        .Select(work => work.Element("Отдел").Value)),
                        maxSalary = g.SelectMany(e => e.Descendants("Зарплата")
                        .Select(salary => (decimal)salary.Element("Размер")))
                        .Max()
                    });
        foreach (var e in employees)
        {
            Console.WriteLine($"Сотрудник: {e.name}");
            Console.WriteLine("Отделы: ");
            foreach (var department in e.departments) Console.WriteLine(department);
            Console.WriteLine($"Максимальная зарплата: {e.maxSalary}");
        }


        Console.WriteLine("\nЗадание 4");
        var departments2 = doc.Element("Сотрудники").Elements("Сотрудник")
            .SelectMany(e => e.Element("Список_Работ").Elements("Работа"))
            .GroupBy(work => work.Element("Отдел").Value)
            .Where(g => g.Count() <= 3)
            .Select(g => g.Key);

        foreach (var department in departments2) Console.WriteLine(department);


        Console.WriteLine("\nЗадание 5");
        var hiringYears = doc.Element("Сотрудники").Elements("Сотрудник")
            .SelectMany(e => e.Element("Список_Работ").Elements("Работа"))
            .GroupBy(work => work.Element("Дата_начала").Value.Split("-")[0])
            .OrderByDescending(g => g.Count()).Take(1)
            .Select(g => new
            {
                Year = g.Key,
                Count = g.Count()
            });

        var firingYears = doc.Element("Сотрудники").Elements("Сотрудник")
            .SelectMany(e => e.Element("Список_Работ").Elements("Работа"))
            .Where(e => e.Element("Дата_окончания").Value != "")
            .GroupBy(r => r.Element("Дата_окончания").Value.Split("-")[0])
            .OrderByDescending(g => g.Count()).Take(1)
            .Select(g => new
            {
                Year = g.Key,
                Count = g.Count()
            });
        foreach (var year in hiringYears)
            Console.WriteLine($"Год : {year.Year} Кол-во нанятых: {year.Count}");
        foreach (var year in firingYears)
            Console.WriteLine($"Год : {year.Year} Кол-во уволенных: {year.Count}");
    }
}
