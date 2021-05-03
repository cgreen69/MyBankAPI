using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBank.API.Infrastructure;
using MyBank.API.Model;
using MyBank.API.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MyBank.API.Controllers
{

    [ApiController]
    [Route("api")]
    public class TransactionController : ControllerBase
    {
        
        private readonly IBankingService transService;

        public TransactionController(IBankingService transService)
        {

            this.transService = transService;
        }

        [HttpGet]
        [Route("transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ITransactionRequest>>> GetTransactionsAsync()
        {

            var transactions = await transService.GetTransactionsAsync();

            return Ok(transactions);


        }

        [HttpPost]
        [Route("process")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ProcessAsync(TransactionRequest tran)
        {
            
            await this.transService.ProcessTransactionAsync(tran);

            return Ok();


        }
    }

}
