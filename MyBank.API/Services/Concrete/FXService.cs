using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyBank.API.Infrastructure;
using MyBank.API.Services.Interface;
using System;
using System.Threading.Tasks;

namespace MyBank.API.Services.Concrete
{

    public class FXService : IFXService
    {


        private readonly IConfiguration configuration;
        private readonly IAPIService apiService;
        private readonly ILogger<FXService> logger;
        private readonly string baseCCY;

        public FXService(IConfiguration configuration, IAPIService apiService,ILogger<FXService> logger)
        {

            baseCCY = configuration.GetValue<string>("MyBankSettings:BaseCCY");

            if (baseCCY is null)
            {

                throw new ArgumentNullException(nameof(baseCCY));
            }

            this.configuration = configuration;

            this.apiService = apiService;

            this.logger = logger;
        }

        public async Task<decimal> GetLatestRateForCCYAsync(string ccy)
        {
            if (ccy == baseCCY) return 1;

            var url = configuration.GetValue<string>("MyBankSettings:RatesUrl");

            var builder = new UriBuilder(url);

            builder.Query = $"base={baseCCY}&symbols={ccy}";

            this.logger.LogInformation($"Obtaining rate for {this.baseCCY}:{ccy}");

            var res = await apiService.GetAsync<ExchangeRate>(builder);

            return res.GetExchangeRate();

        }
    }
}
