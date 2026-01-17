using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Personal_Finance_Manager.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net.NetworkInformation;
using Microsoft.Identity.Client;
using Personal_Finance_Manager.ViewModels;

namespace Personal_Finance_Manager.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<Account>> GetAccountsByUser(string userId)
        {
            using var conn = CreateConnection();


            var accounts = await conn.QueryAsync<Account>(
                "Account.GetAccountsByUser",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );

            return accounts;
        }

        public async Task<Account?> GetAccountById(int accountId, string userId)
        {
            using var conn = CreateConnection();


            var account = await conn.QueryFirstOrDefaultAsync<Account>(
                "Account.GetAccountById",
                new { AccountId = accountId, OwnerUserId = userId},
                commandType: CommandType.StoredProcedure
            );

            return account;
        }


        public async Task<bool> CreateAccount(CreateAccountViewModel account, string userId)
        {
            using var conn = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@OwnerUserId", userId);
            parameters.Add("@Name", account.Name);
            parameters.Add("@AccountTypeId", account.AccountTypeId);
            parameters.Add("@Balance", account.Balance);

            parameters.Add("@ReturnCode",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue
            );

            await conn.ExecuteAsync(
                "Account.CreateAccount",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@ReturnCode") == 0;
        }


        public async Task<bool> UpdateAccount(Account account)
        {
            using var conn = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@Id", account.Id);
            parameters.Add("@OwnerUserId", account.OwnerUserId);
            parameters.Add("@Name", account.Name);
            parameters.Add("@AccountTypeId", account.AccountTypeId);
            parameters.Add("@Balance", account.Balance);

            parameters.Add("@ReturnCode",
              dbType: DbType.Int32,
              direction: ParameterDirection.ReturnValue
          );


            await conn.ExecuteAsync(
                "Account.UpdateAccount",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@ReturnCode") == 0;

        }

        public async Task<bool> DeleteAccount(int accountId, string userId)
        {
            using var conn = CreateConnection();

            var parameters = new DynamicParameters();
            
            parameters.Add("@AccountId", accountId);
            parameters.Add("@OwnerUserId", userId);
            
            parameters.Add("@ReturnCode", 
                dbType: DbType.Int32, 
                direction: ParameterDirection.ReturnValue
            );

            await conn.ExecuteAsync(
                "Account.DeleteAccount",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@ReturnCode") == 0;
        }
    }
}
