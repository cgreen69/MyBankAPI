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
        
        
        private readonly IConfiguration configuration;
        private readonly IAPIService apiService;
        private readonly string baseCCY;

        public RatesService(IConfiguration configuration,IAPIService apiService)
        {

            this.baseCCY  = configuration.GetValue<string>("MyBankSettings:BaseCCY");

            this.configuration = configuration;
            
            this.apiService = apiService;
        }

        public async Task<decimal> GetLatestRateForCCYAsync(string ccy)
        {
            if (ccy == this.baseCCY) return 1;

            var url = this.configuration.GetValue<string>("MyBankSettings:RatesUrl"); 

            var builder = new UriBuilder(url);

            builder.Query = $"base={this.baseCCY}&symbols={ccy}";
            
            var res =  await this.apiService.GetAsync<ExchangeRate>(builder);

            return res.GetExchangeRate();

        }
    }
}
