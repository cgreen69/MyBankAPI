using MyBank.API.Infrastructure;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBank.API.Services
{
    //  https://api.ratesapi.io/api/latest?base=USD&symbols=GBP
    public class RatesService : IRatesService
    {
        private IHttpClientFactory clientFactory;

        const string baseUrl = "https://api.ratesapi.io/api/latest";

        // TODO: make injectible 
        private const string baseCCY = "GBP";

        public RatesService(IHttpClientFactory clientFactory)
        {

            this.clientFactory = clientFactory;
        }

        public async Task<decimal> GetLatestRateForCCYPairAsync(string ccy)
        {
            if (ccy == baseCCY) return 1;

            var builder = new UriBuilder("https://api.ratesapi.io/api/latest");

            builder.Query = $"base={baseCCY}&symbols={ccy}";

            var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);

            //request.Headers.Add("Accept", "application/vnd.github.v3+json");
            //request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            using var client = clientFactory.CreateClient();

            using var response = await client.SendAsync(request);

            // TODO introduce cache of exchange rates to reduce hits on API

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();

                var exRate = await JsonSerializer.DeserializeAsync<ExchangeRate>(responseStream);

                return exRate.GetExchangeRate();

            }
            else
            {
                throw new Exception($"Unable to obtains rates from remote api, status code:{response.StatusCode}");

            }

        }
    }
}
