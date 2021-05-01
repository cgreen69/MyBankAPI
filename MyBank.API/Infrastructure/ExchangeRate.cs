using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBank.API.Infrastructure
{
    public class ExchangeRate
    {

        [JsonPropertyName("base")]
        public string BaseCCY { get; set; }
        [JsonPropertyName("date")]
        public DateTime ConversionDate { get; set; }
        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; }

        public decimal GetExchangeRate()
        {
            return Rates.First().Value;
        }

    }
}
