using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using MyBank.API.Infrastructure;
using MyBank.API.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace MyBank.Tests
{
    public class ExchangeRateTests
    {
        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        [Fact]
        public async void CanDeserializeJsonRates()
        {

            var rawJson = "{\"base\":\"USD\",\"rates\":{\"GBP\":0.720246975},\"date\":\"2021-04-23\"}";

            using var stream = GenerateStreamFromString(rawJson);

            var exRate = await JsonSerializer.DeserializeAsync<ExchangeRate>(stream);

            var rate = exRate.GetExchangeRate();

            rate.Should().Be(0.720246975m);

        }

        [Fact]
        public async void isRateOneForSameBaseAndSymbol() {

            
            IConfiguration configuration = new ConfigurationBuilder()

                .AddInMemoryCollection(new Dictionary<string, string> {

                    {"MyBankSettings:BaseCCY", "GBP" }
    
                })

            .Build();

            var mockRemoteAPI = new Mock<IAPIService>(); 


            var rs = new RatesService(configuration,mockRemoteAPI.Object); 

            var res = await rs.GetLatestRateForCCYAsync("GBP");

            res.Should().Be(1);

            
        }

        

        [Fact]
        public async void isRateObtainedForDifferentBaseAndSymbol() {


            IConfiguration configuration = new ConfigurationBuilder()

                .AddInMemoryCollection(new Dictionary<string, string> {

                    {"MyBankSettings:BaseCCY", "EUR" },
                    {"MyBankSettings:RatesUrl","testurl" }
    
                })

            .Build();


            var mockRemoteAPI = new Mock<IAPIService>(); 

            var rateDictionary = new Dictionary<string,decimal>() { {"EUR",0.75m } };

            var exRate = new ExchangeRate() {Rates = rateDictionary };

            mockRemoteAPI.Setup(m=>m.GetAsync<ExchangeRate>(It.IsAny<UriBuilder>())).Returns(Task.FromResult(exRate));

            var rs = new RatesService(configuration,mockRemoteAPI.Object); 

            var res = await rs.GetLatestRateForCCYAsync("GBP");

           res.Should().Be(0.75m);

        }


    }
}
