using Moq;
using MyBank.API.Model;
using System;
using FluentAssertions;
using Xunit;
using System.Threading.Tasks;
using MyBank.API.Repositories;
using MyBank.API.Services.Interface;
using Microsoft.Extensions.Logging;
using MyBank.API.Services.Concrete;
using MyBank.API.Infrastructure;

namespace MyBank.Tests
{
    public class BankingServiceTests

    {
        private readonly ILogger<BankingService> logger = new Mock<ILogger<BankingService>>().Object;

        [Fact]
        public async void DoesAppliedFXRateSetCorrectTransactionAmount() {


            var fx = new Mock<IFXService>();

            fx.Setup(f=>f.GetLatestRateForCCYAsync(It.Is<string>(s=>s=="USD"))).Returns(Task.FromResult<decimal>(1.2m));

            var tr = new Mock<ITransactionRepository>();

            tr.Setup(t=>t.GetCurrentBalanceAsync()).Returns(Task.FromResult(1000m));

            var bs = new BankingService(tr.Object,fx.Object,this.logger);

            var trans = new Mock<ITransactionRequest>();

            trans.Setup(t=>t.Amount).Returns(1000);

            trans.Setup(t=>t.Ccy).Returns("USD");

            await bs.ProcessTransactionAsync(trans.Object);

            tr.Verify(t=>t.InsertAsync(It.Is<ITransaction>(t=> Math.Round(t.Amount,2) == 833.33m)),Times.Once);
            


        }


        [Fact]
        public void DoesNegativeTransactionThrowException() {

            var bs = new BankingService(null,null,null);

            var trans = new Mock<ITransactionRequest>();

            trans.Setup(t=>t.Amount).Returns(-123.45m);

            Func<Task> t = async () => {

                await bs.ProcessTransactionAsync(trans.Object);

            };

            t.Should().Throw<Exception>();

        }

        [Fact]
        public void CannotHaveNegativeFunds() {

           
            var fx = new Mock<IFXService>();

            fx.Setup(f=>f.GetLatestRateForCCYAsync(It.Is<string>(s=>s=="GBP"))).Returns(Task.FromResult<decimal>(1));

            var tr = new Mock<ITransactionRepository>();

            tr.Setup(t=>t.GetCurrentBalanceAsync()).Returns(Task.FromResult(1000m));

            var bs = new BankingService(tr.Object,fx.Object,this.logger);

            var trans = new Mock<ITransactionRequest>();

            trans.Setup(t=>t.Amount).Returns(10000);

            trans.Setup(t=>t.TransactionType).Returns(TransactionType.Withdrawal);

            trans.Setup(t=>t.Ccy).Returns("GBP");

            Func<Task> t = async () => {

                await bs.ProcessTransactionAsync(trans.Object);

            };

    
           t.Should().Throw<Exception>().WithMessage("*negative*");

        }

        
        [Fact]
        public void IsMaximumTransactionAmountEnforced() {

            var bs = new BankingService(null,null,null);

            var trans = new Mock<ITransactionRequest>();

            trans.Setup(t=>t.Amount).Returns(500000000000000);

            Func<Task> t = async () => {

                await bs.ProcessTransactionAsync(trans.Object);

            };

            t.Should().Throw<Exception>().WithMessage("*maximum*");

        }

    }
}
