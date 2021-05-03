using Microsoft.Extensions.Logging;
using MyBank.API.Infrastructure;
using MyBank.API.Model;
using MyBank.API.Repositories;
using MyBank.API.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBank.API.Services.Concrete
{


    public class BankingService : IBankingService
    {

        private readonly ITransactionRepository transRepo;
        private readonly IFXService ratesService;
        private readonly ILogger<BankingService> logger;
        private const int MaximumAmount = 50000;

        public BankingService(ITransactionRepository transRepo, IFXService ratesService, ILogger<BankingService> logger)
        {
            this.transRepo = transRepo;
            this.ratesService = ratesService;
            this.logger = logger;
        }



        public async Task<IEnumerable<ITransaction>> GetTransactionsAsync()
        {

            return await transRepo.GetTransactionsAsync();
        }


        public async Task ProcessTransactionAsync(ITransactionRequest trans)
        {

            if (trans.Amount <= 0) throw new Exception("Unable to process transactions with zero or negative values");

            if (trans.Amount > 50000) throw new Exception($"Maximum amount of {MaximumAmount} exceeded");

            if (trans.Ccy is null)
            {

                throw new ArgumentNullException(nameof(trans.Ccy));
            }


            var currentBalance = await transRepo.GetCurrentBalanceAsync();

            var latestRate = await ratesService.GetLatestRateForCCYAsync(trans.Ccy);

            this.logger.LogInformation($"fx rate={latestRate}");

            var rateAdjustedAmount = trans.Amount / latestRate;

            var pendingAmount = trans.TransactionType == TransactionType.Deposit ? rateAdjustedAmount : rateAdjustedAmount * -1;

            var fullTransaction = new Transaction()
            {
                Date = DateTime.UtcNow,
                Description = Enum.GetName(typeof(TransactionType),trans.TransactionType) + (latestRate==1 ? string.Empty : $" {trans.Amount} {trans.Ccy}@ {latestRate}"),
                Amount = pendingAmount,
                Balance = currentBalance += pendingAmount
            };

            if (fullTransaction.Balance < 0) throw new Exception("bank funds cannot be negative");

            this.logger.LogInformation($"saving transaction - {fullTransaction}");

            await transRepo.InsertAsync(fullTransaction);

        }

    }
}
