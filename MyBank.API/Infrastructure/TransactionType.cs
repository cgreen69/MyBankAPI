using System.Text.Json.Serialization;

namespace MyBank.API.Infrastructure
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionType
    {
        Deposit,Withdrawal

    }
}
