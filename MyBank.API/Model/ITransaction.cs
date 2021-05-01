using System;

namespace MyBank.API.Model
{
    public interface ITransaction
    {
        decimal Amount { get; set; }
        decimal Balance { get; set; }
        DateTime Date { get; set; }
        string Description { get; set; }
        long Id { get; set; }
        string Name { get; set; }
    }
}