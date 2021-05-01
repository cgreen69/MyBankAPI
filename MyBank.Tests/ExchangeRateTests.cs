using FluentAssertions;
using MyBank.API.Infrastructure;
using System.IO;
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
    }
}
