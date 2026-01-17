using Personal_Finance_Manager.Models;
using Personal_Finance_Manager.ViewModels;

namespace Personal_Finance_Manager.Data.Repositories
{
    public interface ITransactionRepository
    {

        Task<IEnumerable<Transaction>> GetTransactionsByAccount(int accountId, string userId);

        Task<Transaction?> GetTransactionById(int transactionId, string userId);

        Task<bool> CreateTransaction(CreateTransactionViewModel transaction, string userId);

        Task<bool> DeleteTransaction(int transactionId, string userId);

        Task<bool> UndoDeleteTransaction(string ownerUserId, int transactionId);
    }
}
