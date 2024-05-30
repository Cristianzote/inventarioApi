using inventarioApi.Data.Models;
using inventarioApi.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventarioApi.Controllers
{
    [ApiController]
    [Route("/api/v1/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        //GET
        [HttpGet()]
        [AllowAnonymous]
        public async Task<ActionResult<Inventory>> GetTransaction()
        {
            var transactions = await _transactionService.GetTransactions();

            if (transactions == null)
            {
                return NotFound();
            }
            return Ok(transactions);
        }


        //POST
        [HttpPost()]
        [AllowAnonymous]
        public async Task<ActionResult> CreateTransaction([FromBody] Transaction TRANSACTION)
        {
            var transaction = await _transactionService.CreateTransaction(TRANSACTION);
            return Ok(transaction);
        }
    }
}
