using MyBank.API.Infrastructure;
using System;

namespace MyBank.API.Model
{

    public interface ITransactionRequest
    {
        decimal Amount { get; set; }
        string Ccy {get;set;}
        TransactionType TransactionType { get; set; }
        
        
    }

}
