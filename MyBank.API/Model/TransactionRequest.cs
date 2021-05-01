

using MyBank.API.Infrastructure;
using System.Text.Json.Serialization;

namespace MyBank.API.Model
{
    public class TransactionRequest : ITransactionRequest
    {
        
        public TransactionType TransactionType {get;set;}
        public decimal Amount { get;set; }
        public string Ccy { get;set; }
    }
}
