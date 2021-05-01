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
        private ILogger<string> _logger;
        private readonly IBankingService transService;

        public TransactionController(ILogger<string> logger, IBankingService transService)
        {
            _logger = logger;
            this.transService = transService;
        }

        [HttpGet]
        [Route("transactions")]
        public async Task<ActionResult<IEnumerable<ITransactionRequest>>> GetTransactionsAsync()
        {

            var list = await transService.GetTransactionsAsync();

            return Ok(list);

            //return new List<string> { "alpha", "beta" };

            //var sql = "[PRG].[csp_API_GetPeerGroupGetDuplicateNamesCount]";
            //var values = new { UserId = duplicateNameRequest.UserId, Name = duplicateNameRequest.PeerGroupName };
            //await using var conn = new SqlConnection(_connectionString.Value);
            //var result = await conn.QueryAsync<int>(sql, values, commandType: CommandType.StoredProcedure);


        }

        [HttpPost]
        [Route("process")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ProcessAsync(TransactionRequest tran)
        {
            
            switch (tran.TransactionType) {
                case TransactionType.Withdrawal:
                    
                case TransactionType.Deposit:
                    await this.transService.ProcessTransactionAsync(tran);
                    break;

                 default:
                    throw new Exception("Transaction type not supported");
                  
            }


            return Ok(true);


        }
    }

}
