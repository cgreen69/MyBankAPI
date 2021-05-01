using MyBank.API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBank.API.Services.Interface
{
    public interface IBankingService
    {
        Task<IEnumerable<ITransaction>> GetTransactionsAsync();
        Task<bool> ProcessTransactionAsync(ITransactionRequest trans);
    }
}