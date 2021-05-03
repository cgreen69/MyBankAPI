using System.Threading.Tasks;

namespace MyBank.API.Services.Interface
{
    public interface IFXService
    {
        Task<decimal> GetLatestRateForCCYAsync(string ccy);
    }
}