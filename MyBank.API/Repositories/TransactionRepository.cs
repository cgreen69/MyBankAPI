using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using MyBank.API.Infrastructure;
using MyBank.API.Model;

namespace MyBank.API.Repositories
{
    /// <summary>
    /// I am responsbile for persisting and retrieving transactions
    /// </summary>
    public class TransactionRepository : ITransactionRepository
    {
        private readonly string _connectionString;
        private readonly ILogger logger;

        public TransactionRepository(DatabaseOptions options, ILogger<TransactionRepository> logger)
        {
            _connectionString = options.ConnectionString;
            this.logger = logger;
        }

        public async Task<IEnumerable<ITransaction>> GetTransactionsAsync()
        {
            const string query = "select * from [transaction] order by [date] ASC";

            logger.LogInformation("getting transactions");

            await using var conn = new SqlConnection(_connectionString);  

            conn.Open();

            var result = await conn.QueryAsync<Transaction>(query, commandType: CommandType.Text);

            return result;
        }

        public async Task<decimal> GetCurrentBalanceAsync() {

            const string query = "select top 1 balance from [transaction] order by [date] DESC";

            logger.LogInformation("getting current balance");

            await using var conn = new SqlConnection(_connectionString);

            conn.Open();

            var result = await conn.QueryAsync<decimal>(query, commandType: CommandType.Text);

            if (!result?.Any() ?? true) { return 0m ;}

            return result.First();
        }

        public async Task InsertAsync(ITransaction transaction)
        {

            string query = $"insert into [transaction] ([date],description,amount,balance) values ('{transaction.Date:yyyy-MM-dd HH:mm:ss.fff}','{transaction.Description}',{transaction.Amount},{transaction.Balance})";

            await using var conn = new SqlConnection(_connectionString);

            conn.Open();

            await conn.ExecuteAsync(query, commandType: CommandType.Text);

            logger.LogInformation("save successful");
            
        }
    }
}
