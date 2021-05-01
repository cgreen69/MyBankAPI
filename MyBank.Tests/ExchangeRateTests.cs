using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using MyBank.API.Infrastructure;
using MyBank.API.Services;
using System.IO;
using System.Net.Http;
using System.Text.Json;
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

            var mock = new Mock<IBaseCurrency>(); 

            mock.Setup(x=>x.Currency).Returns("GBP");

            var rs = new RatesService(mock.Object,null,null); 

            var res = await rs.GetLatestRateForCCYAsync("GBP");

            res.Should().Be(1);

            
        }

        

        [Fact]
        public async void isRateObtainedForDifferentBaseAndSymbol() {

            var mockCCY = new Mock<IBaseCurrency>(); 

            mockCCY.Setup(x=>x.Currency).Returns("GBP");

            var mockRemoteAPI = new Mock<IHttpClientFactory>(); 

            mockCCY.Setup(x=>x.Currency).Returns("GBP");

            var mockConfiguration = new Mock<IConfiguration>(); 

            mockConfiguration.Setup(m=>m.GetValue<string>(It.Is<string>(i=>i == "MyBankSettings:RatesUrl" ))).Returns("testurl");

            //var rs = new RatesService(mockRemoteAPI.Object,mockCCY.Object,mockConfiguration.Object); 

            //var res = await rs.GetLatestRateForCCYAsync("GBP");

          // res.Should().Be(1);

        }


    }
}
