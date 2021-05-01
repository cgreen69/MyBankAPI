using System.Threading.Tasks;

namespace MyBank.API.Services
{
    public interface IRatesService
    {

        Task<decimal> GetLatestRateForCCYPairAsync(string ccy);
    }
}