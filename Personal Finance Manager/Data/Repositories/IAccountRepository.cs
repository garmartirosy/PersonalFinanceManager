using Personal_Finance_Manager.Models;
using Personal_Finance_Manager.ViewModels;

namespace Personal_Finance_Manager.Data.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccountsByUser(string userId);

        Task<Account?> GetAccountById(int accountId, string userId);

        Task<bool> CreateAccount(CreateAccountViewModel account, string userId);

        Task<bool> UpdateAccount(Account account);

        Task<bool> DeleteAccount(int accountId, string userId);
    }
}
