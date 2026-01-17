using System;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Personal_Finance_Manager.Models;
using Personal_Finance_Manager.ViewModels;

namespace Personal_Finance_Manager.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly string _connectionString;

        public TransactionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }


        public async Task<IEnumerable<Transaction>> GetTransactionsByAccount(int accountId, string userId)
        {
            using var conn = CreateConnection();

            var transactions = await conn.QueryAsync<Transaction>(
                "Account.GetTransactionsByAccount",
                new { AccountId = accountId, OwnerUserId = userId },
                commandType: CommandType.StoredProcedure
            );

            return transactions;
        }

        public async Task<Transaction?> GetTransactionById(int transactionId, string userId)
        {
            using var conn = CreateConnection();

            var transaction = await conn.QueryFirstOrDefaultAsync<Transaction>(
                "Account.GetTransactionById",
                new { TransactionId = transactionId, OwnerUserId = userId },
                commandType: CommandType.StoredProcedure
            );

            return transaction;
        }

        public async Task<bool> CreateTransaction(CreateTransactionViewModel transaction, string userId)
        {
            using var conn = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@OwnerUserId", userId);
            parameters.Add("@AccountId", transaction.AccountId);
            parameters.Add("@Description", transaction.Description);
            parameters.Add("@DestinationName", transaction.DestinationName);
            parameters.Add("@TransactionTypeId", transaction.TransactionTypeId);
            parameters.Add("@Amount", transaction.Amount);

            parameters.Add("@TransactionId",
                dbType: DbType.Int32,
                direction: ParameterDirection.Output
            );

            parameters.Add("@ReturnCode",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue
            );

            await conn.ExecuteAsync(
                "Account.CreateTransaction",   
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int returnCode = parameters.Get<int>("@ReturnCode");


            return returnCode == 0;
        }


        public async Task<bool> UpdateTransaction(Transaction transaction, string userId)
        {
            using var conn = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@TransactionId", transaction.Id);
            parameters.Add("@OwnerUserId", userId);

            parameters.Add("@AccountId", transaction.AccountId);
            parameters.Add("@Description", transaction.Description);
            parameters.Add("@DestinationName", transaction.DestinationName);
            parameters.Add("@TransactionTypeId", transaction.TransactionTypeId);
            parameters.Add("@Amount", transaction.Amount);

            parameters.Add("@ReturnCode",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue
            );

            await conn.ExecuteAsync(
                "Account.UpdateTransaction",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@ReturnCode") == 0;
        }

        public async Task<bool> DeleteTransaction(int transactionId, string userId)
        {
            using var conn = CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@TransactionId", transactionId);
            parameters.Add("@OwnerUserId", userId);

            parameters.Add("@ReturnCode",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue
            );

            await conn.ExecuteAsync(
                "Account.DeleteTransaction",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@ReturnCode") == 0;
        }
        public async Task<bool> UndoDeleteTransaction(string ownerUserId, int transactionId)
        {
            using var connection = CreateConnection();

            var p = new DynamicParameters();
            p.Add("@OwnerUserId", ownerUserId);
            p.Add("@TransactionId", transactionId);
            p.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                "Account.UndoDeleteTransaction",
                p,
                commandType: CommandType.StoredProcedure
            );

            return p.Get<int>("@ReturnVal") == 0;
        }

    }
}