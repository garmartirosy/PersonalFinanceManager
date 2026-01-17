using System.Text;
using Personal_Finance_Manager.Models;
using Personal_Finance_Manager.Services;
using Xunit;

namespace PersonalFinanceManager.Tests.Services.Reports
{
    public class ReportGeneratorTests
    {
        [Fact]
        public async Task GenerateReport_Csv_ReturnsCsvFile()
        {
            var reportGenerator = new ReportGenerator();

            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, Description = "Coffee", Amount = 3.50m }
            };

            var result = await reportGenerator.GenerateReport("csv", transactions, "My Account");

            Assert.NotNull(result);
            Assert.Equal("text/csv", result.ContentType);
            Assert.EndsWith(".csv", result.FileName);
            Assert.NotNull(result.Bytes);
            Assert.True(result.Bytes.Length > 0);
        }

        [Fact]
        public async Task GenerateReport_Json_ReturnsJsonFile()
        {
            var reportGenerator = new ReportGenerator();

            var transactions = new List<Transaction>
            {
                new Transaction { Id = 2, Description = "Lunch", Amount = 12.00m }
            };

            var result = await reportGenerator.GenerateReport("json", transactions, "My Account");

            Assert.NotNull(result);
            Assert.Equal("application/json", result.ContentType);
            Assert.EndsWith(".json", result.FileName);

            var jsonText = Encoding.UTF8.GetString(result.Bytes);
            Assert.Contains("AccountName", jsonText);
            Assert.Contains("My Account", jsonText);
        }

        [Fact]
        public async Task GenerateReport_InvalidType_ThrowsException()
        {
            var reportGenerator = new ReportGenerator();
            var transactions = new List<Transaction>();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                reportGenerator.GenerateReport("xml", transactions, "My Account")
            );
        }

        [Fact]
        public async Task GenerateReport_NullType_ThrowsException()
        {
            var reportGenerator = new ReportGenerator();
            var transactions = new List<Transaction>();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                reportGenerator.GenerateReport(null, transactions, "My Account")
            );
        }
    }
}
