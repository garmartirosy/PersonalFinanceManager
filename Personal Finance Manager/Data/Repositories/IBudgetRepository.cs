using Personal_Finance_Manager.Models;

namespace Personal_Finance_Manager.Data.Repositories
{
    public interface IBudgetRepository
    {
        Task<int> CreateBudget(Budget budget);
        Task<int> DeleteBudget(int budgetId, string ownerUserId);

        Task<Budget?> GetBudgetById(int budgetId, string ownerUserId);
        Task<IEnumerable<Budget>> GetBudgetsByUser(string ownerUserId);

        Task<int> CreateBudgetEntry(BudgetEntry entry);
        Task<int> DeleteBudgetEntry(int budgetEntryId, string ownerUserId);

        Task<BudgetEntry?> GetBudgetEntryById(int budgetEntryId, string ownerUserId);
        Task<IEnumerable<BudgetEntry>> GetBudgetEntriesByBudget(int budgetId, string ownerUserId);

        Task<decimal> GetTotalSpent(int budgetId);

    }
}
