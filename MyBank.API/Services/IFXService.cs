using System.Threading.Tasks;

namespace MyBank.API.Services
{
    public interface IFXService
    {

        Task<decimal> GetLatestRateForCCYAsync(string ccy);
    }
}