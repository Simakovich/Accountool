using Accountool.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Accountool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Parser.Parse();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public static class Parser
    {
        public static void Parse()
        {
            // ��� ����� CSV
            string csvFile = "D:\\Univer\\Diplom\\Diplom\\Accountool\\Store\\DataTvs.csv";

            // ������ �����
            string[] lines = File.ReadAllLines(csvFile);
            
            // ������� SQL ������
            string queryTemplate = "INSERT INTO Indication (Month, Value, Tarif1, Tarif2, TarifSumm, Archive, SchetchikId) VALUES "
                                 + "('{0}', {1}, 0.2537, 0.3216, 0.5, 0, (SELECT Id FROM Kiosk WHERE Name = '����� �{2}'))";

            StringBuilder sqlQueries = new StringBuilder();

            for (int i = 1; i < lines.Length; i++)
            {
                // ���������� ������ �� �� ������������ �����
                string[] parts = lines[i].Split("\t");

                // �������� ������
                string kioskName = parts[0].Replace("�����", "");

                for (int j = 1; j < parts.Length; j++)
                {
                    // ��������� ������
                    decimal decValue = Decimal.Parse(parts[j], CultureInfo.InvariantCulture);

                    // ���� (�� ������ ������ ������)
                    string date = new DateTime(2018 + ((j - 1) / 12), ((j - 1) % 12) + 1, 1).ToString("yyyy-MM-dd");

                    // �������� SQL �������
                    string query = string.Format(queryTemplate, date, decValue.ToString(CultureInfo.InvariantCulture), kioskName);

                    // ���������� SQL �������
                    sqlQueries.AppendLine(query);
                }
            }

            // ������ SQL �������� � ����
            File.WriteAllText("queries.sql", sqlQueries.ToString());
        }
    }
}
