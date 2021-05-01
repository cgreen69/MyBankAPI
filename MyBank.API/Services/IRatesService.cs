using System.Threading.Tasks;

namespace MyBank.API.Services
{
    public interface IRatesService
    {

        Task<decimal> GetLatestRateForCCYAsync(string ccy);
    }
}