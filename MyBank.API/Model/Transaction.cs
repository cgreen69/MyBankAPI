using System;

namespace MyBank.API.Model
{
    /// <summary>
    /// I represent a transaction, a single entry on a banking statement
    /// </summary>
    public class Transaction : ITransaction
    {
        public override string ToString()
        {
            return $"{Description}:{Amount}";
        }
        public long Id { get; set; }
        
        public DateTime Date { get;set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
    }
}
