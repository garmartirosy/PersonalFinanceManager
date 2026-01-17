using Personal_Finance_Manager.Models;

namespace Personal_Finance_Manager.Services.Reports
{
    public interface IReport
    {
        string ReportType { get; } 
        Task<ReportFile> Export(List<Transaction> transactions, string accountName);
    }
}
