using Microsoft.Extensions.Configuration;
using MyBank.API.Infrastructure;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBank.API.Services
{
    
    public class FXService : IFXService
    {
        
        
        private readonly IConfiguration configuration;
        private readonly IAPIService apiService;
        private readonly string baseCCY;

        public FXService(IConfiguration configuration,IAPIService apiService)
        {

            this.baseCCY  = configuration.GetValue<string>("MyBankSettings:BaseCCY");

            if (this.baseCCY is null) {

                throw new ArgumentNullException(nameof(this.baseCCY));
            }

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
