using CsvHelper;
using Personal_Finance_Manager.Models;
using Personal_Finance_Manager.Services.Reports;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Personal_Finance_Manager.Services
{
    public class ReportGenerator
    {
        private Dictionary<string, IReport> exporters;

        public ReportGenerator()
        {
            exporters = new Dictionary<string, IReport>();

            exporters["csv"] = new CsvExporter();
            exporters["json"] = new JsonExporter();
        }

        public Task<ReportFile> GenerateReport(string reportType, List<Transaction> transactions, string accountName)
        {
            var exporter = GetExporter(reportType);
            return exporter.Export(transactions, accountName);
        }

        private IReport GetExporter(string reportType)
        {
            reportType = reportType.ToLower();
            return exporters[reportType];
        }



        private class CsvExporter : IReport
        {
            public string ReportType => "csv";

            public Task<ReportFile> Export(List<Transaction> transactions, string accountName)
            {
                using var ms = new MemoryStream();
                using var writer = new StreamWriter(ms, Encoding.UTF8);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

                csv.WriteRecords(transactions);
                writer.Flush();

                var bytes = ms.ToArray();

                string safeName = MakeSafeFileName(accountName);
                string fileName = "transactions_" + safeName + ".csv";

                return Task.FromResult(new ReportFile(bytes, "text/csv", fileName));
            }
        }

        private class JsonExporter : IReport
        {
            public string ReportType => "json";

            public Task<ReportFile> Export(List<Transaction> transactions, string accountName)
            {
                string safeName = MakeSafeFileName(accountName);
                string fileName = "transactions_" + safeName + ".json";

                var report = new
                {
                    AccountName = accountName,
                    GeneratedAtUtc = DateTime.UtcNow,
                    Transactions = transactions
                };

                string json = JsonSerializer.Serialize(report, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                byte[] bytes = Encoding.UTF8.GetBytes(json);

                return Task.FromResult(new ReportFile(bytes, "application/json", fileName));
            }
        }

        private static string MakeSafeFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = "account";

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }

            return name;
        }
    }
}
