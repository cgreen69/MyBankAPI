using Microsoft.Extensions.Configuration;
using MyBank.API.Infrastructure;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBank.API.Services
{
    //  https://api.ratesapi.io/api/latest?base=USD&symbols=GBP
    public class RatesService : IRatesService
    {
        
        private readonly IBaseCurrency baseCurrency;
        private readonly IConfiguration configuration;
        private readonly IAPIService apiService;

        public RatesService(IBaseCurrency baseCurrency,IConfiguration configuration,IAPIService apiService)
        {

            this.baseCurrency = baseCurrency;
            this.configuration = configuration;
            this.apiService = apiService;
        }

        public async Task<decimal> GetLatestRateForCCYAsync(string ccy)
        {
            if (ccy == this.baseCurrency.Currency) return 1;

            var url = this.configuration.GetValue<string>("MyBankSettings:RatesUrl"); 

            var builder = new UriBuilder(url);

            builder.Query = $"base={this.baseCurrency.Currency}&symbols={ccy}";
            
            var res =  await this.apiService.GetAsync<ExchangeRate>(builder);

            return res.GetExchangeRate();

        }
    }
}
