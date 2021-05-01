using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBank.API.Model
{
    public class Transaction : ITransaction
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get;set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
    }
}
