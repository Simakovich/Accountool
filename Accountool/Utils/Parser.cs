using Accountool.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Accountool.Utils
{
    public static class Parser
    {
        public static void Parse()
        {
            // Имя файла CSV
            string csvFile = "D:\\Univer\\Diplom\\Diplom\\Accountool\\Store\\DataTvs.csv";

            // Чтение файла
            string[] lines = File.ReadAllLines(csvFile);

            // Базовый SQL запрос
            string queryTemplate = "INSERT INTO Indication (Month, Value, Tarif1, Tarif2, TarifSumm, Archive, SchetchikId) VALUES "
                                 + "('{0}', {1}, 0.2537, 0.3216, 0.5, 0, (SELECT Id FROM Place WHERE Name = 'Киоск №{2}'))";

            StringBuilder sqlQueries = new StringBuilder();

            for (int i = 1; i < lines.Length; i++)
            {
                // Разделение строки на ее составляющие части
                string[] parts = lines[i].Split("\t");

                // Название киоска
                string kioskName = parts[0].Replace("Киоск№", "");

                for (int j = 1; j < parts.Length; j++)
                {
                    // Показания киоска
                    decimal decValue = Decimal.Parse(parts[j], CultureInfo.InvariantCulture);

                    // Дата (на основе номера месяца)
                    string date = new DateTime(2018 + ((j - 1) / 12), ((j - 1) % 12) + 1, 1).ToString("yyyy-MM-dd");

                    // Создание SQL запроса
                    string query = string.Format(queryTemplate, date, decValue.ToString(CultureInfo.InvariantCulture), kioskName);

                    // Добавление SQL запроса
                    sqlQueries.AppendLine(query);
                }
            }

            // Запись SQL запросов в файл
            File.WriteAllText("queries.sql", sqlQueries.ToString());
        }
    }
}
