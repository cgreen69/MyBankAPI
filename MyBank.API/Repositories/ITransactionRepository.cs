using MyBank.API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBank.API.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<ITransaction>> GetTransactionsAsync();
        Task<bool> InsertAsync(ITransaction transaction);
        Task<decimal> GetCurrentBalance();
    }
}