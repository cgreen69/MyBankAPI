using MyBank.API.Infrastructure;
using MyBank.API.Model;
using MyBank.API.Repositories;
using MyBank.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Services
{


    public class BankingService : IBankingService
    {



        private ITransactionRepository transRepo { get; }
        public IFXService ratesService { get; }

        public BankingService(ITransactionRepository transRepo, IFXService ratesService)
        {
            this.transRepo = transRepo;
            this.ratesService = ratesService;
        }



        public async Task<IEnumerable<ITransaction>> GetTransactionsAsync()
        {

            return await this.transRepo.GetTransactionsAsync();
        }

        public async Task<bool> ProcessTransactionAsync(ITransactionRequest trans)
        {

            if (trans.Amount <= 0) throw new Exception("Unable to process transactions with zero or negative values");

            var latestRate = await this.ratesService.GetLatestRateForCCYAsync(trans.Ccy);

            var rateAdjustedAmount = trans.Amount * latestRate;

            var pendingAmount = (trans.TransactionType == TransactionType.Deposit ? rateAdjustedAmount : rateAdjustedAmount * -1);

            var currentBalance = await this.transRepo.GetCurrentBalance();

            var fullTransaction = new Transaction()
            {
                Date = DateTime.UtcNow,
                Description = (trans.TransactionType == TransactionType.Deposit ? "deposit" : "withdrawal"),
                Amount = pendingAmount,
                Balance = currentBalance += pendingAmount
            };

            await this.transRepo.InsertAsync(fullTransaction);


            return true;
        }

    }
}
