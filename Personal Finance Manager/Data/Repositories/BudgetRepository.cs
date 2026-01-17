using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Personal_Finance_Manager.Models;

namespace Personal_Finance_Manager.Data.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly string _connectionString;

        public BudgetRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private static async Task<int> ExecuteWithReturnCode(SqlConnection connection, string storedProcName, DynamicParameters parameters)
        {
            parameters.Add("@ReturnCode", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                storedProcName,
                parameters,
                commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@ReturnCode");
        }

   
        public async Task<int> CreateBudget(Budget budget)
        {
            using var connection = new SqlConnection(_connectionString);

            var p = new DynamicParameters();
            p.Add("@OwnerUserId", budget.OwnerUserId);
            p.Add("@Name", budget.Name);
            p.Add("@Balance", budget.Balance);

            var returnCode = await ExecuteWithReturnCode(connection, "Account.CreateBudget", p);

      
            return returnCode; 
        }

        public async Task<int> UpdateBudget(Budget budget)
        {
            using var connection = new SqlConnection(_connectionString);

            var p = new DynamicParameters();
            p.Add("@Id", budget.Id);
            p.Add("@OwnerUserId", budget.OwnerUserId);
            p.Add("@Name", budget.Name);
            p.Add("@Balance", budget.Balance);

            return await ExecuteWithReturnCode(connection, "Account.UpdateBudget", p);
        }

        public async Task<int> DeleteBudget(int budgetId, string ownerUserId)
        {
            using var connection = new SqlConnection(_connectionString);

            var p = new DynamicParameters();
            p.Add("@Id", budgetId);
            p.Add("@OwnerUserId", ownerUserId);

            return await ExecuteWithReturnCode(connection, "Account.DeleteBudget", p);
        }

        public async Task<Budget?> GetBudgetById(int budgetId, string ownerUserId)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<Budget>(
                "Account.GetBudgetById",
                new
                {
                    Id = budgetId,
                    OwnerUserId = ownerUserId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Budget>> GetBudgetsByUser(string ownerUserId)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<Budget>(
                "Account.GetBudgetsByUser",
                new { OwnerUserId = ownerUserId },
                commandType: CommandType.StoredProcedure);
        }

       
        public async Task<int> CreateBudgetEntry(BudgetEntry entry)
        {
            using var connection = new SqlConnection(_connectionString);

            var p = new DynamicParameters();
            p.Add("@BudgetId", entry.BudgetId);
            p.Add("@OwnerUserId", entry.OwnerUserId);
            p.Add("@Name", entry.Name);
            p.Add("@Amount", entry.Amount);

            var returnCode = await ExecuteWithReturnCode(connection, "Account.CreateBudgetEntry", p);



            return returnCode; 
        }


        public async Task<int> DeleteBudgetEntry(int budgetEntryId, string ownerUserId)
        {
            using var connection = new SqlConnection(_connectionString);

            var p = new DynamicParameters();
            p.Add("@BudgetEntryId", budgetEntryId);
            p.Add("@OwnerUserId", ownerUserId);

            return await ExecuteWithReturnCode(connection, "Account.DeleteBudgetEntry", p);
        }

        public async Task<BudgetEntry?> GetBudgetEntryById(int budgetEntryId, string ownerUserId)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryFirstOrDefaultAsync<BudgetEntry>(
                "Account.GetBudgetEntryById",
                new
                {
                    BudgetEntryId = budgetEntryId,
                    OwnerUserId = ownerUserId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<BudgetEntry>> GetBudgetEntriesByBudget(int budgetId, string ownerUserId)
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<BudgetEntry>(
                "Account.GetBudgetEntriesByBudget",
                new
                {
                    BudgetId = budgetId,
                    OwnerUserId = ownerUserId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<decimal> GetTotalSpent(int budgetId)
        {
            using var connection = new SqlConnection(_connectionString);

            var totalSpent = await connection.QuerySingleAsync<decimal>(
                "Account.GetTotalSpent",
                new { BudgetId = budgetId },
                commandType: CommandType.StoredProcedure);

            return totalSpent;
        }
    }
}
