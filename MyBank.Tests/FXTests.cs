using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MyBank.API.Infrastructure;
using MyBank.API.Services.Concrete;
using MyBank.API.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace MyBank.Tests
{
    public class FXTests
    {
        private readonly ILogger<FXService> logger = new Mock<ILogger<FXService>>().Object;

        private static Stream GenerateStreamFromString(string s)
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
        public async void IsRateOneForSameBaseAndSymbol() {

            
            IConfiguration configuration = new ConfigurationBuilder()

                .AddInMemoryCollection(new Dictionary<string, string> {

                    {"MyBankSettings:BaseCCY", "GBP" }
    
                })

            .Build();

            var mockRemoteAPI = new Mock<IAPIService>(); 


            var rs = new FXService(configuration,mockRemoteAPI.Object,this.logger); 

            var res = await rs.GetLatestRateForCCYAsync("GBP");

            res.Should().Be(1);

            
        }

        

        [Fact]
        public async void IsRateObtainedForDifferentBaseAndSymbol() {


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

            var rs = new FXService(configuration,mockRemoteAPI.Object,this.logger); 

            var res = await rs.GetLatestRateForCCYAsync("GBP");

           res.Should().Be(0.75m);

        }


    }
}
