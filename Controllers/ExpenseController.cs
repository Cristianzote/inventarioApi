using inventarioApi.Data.Jobs;
using inventarioApi.Data.Models;
using inventarioApi.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace inventarioApi.Controllers
{
    [ApiController]
    [Route("/api/v1/expense")]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseService _expenseService;

        public ExpenseController(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        //GET
        [HttpGet("{ID}")]
        [AllowAnonymous]
        public async Task<ActionResult<Expense>> GetExpense(int ID)
        {
            var expenses = await _expenseService.GetExpenses();
            var expense = expenses.Where(e => e.IdExpense == ID).FirstOrDefault();

            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Expense>> GetExpenses()
        {
            var expenses = await _expenseService.GetExpenses();

            if (expenses == null)
            {
                return NotFound();
            }
            return Ok(expenses);
        }

        //POST
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Expense>> CreateExpense([FromBody] Expense EXPENSE)
        {
            var expense = await _expenseService.CreateExpense(EXPENSE);
            return Ok(expense);
        }

        //UPDATE
        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateExpense([FromBody] Expense EXPENSE)
        {
            var expense = await _expenseService.UpdateExpense(EXPENSE);

            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }

        [HttpPut("disable/{ID}")]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateExpense(int ID)
        {
            var expenses = await _expenseService.GetExpenses();
            var expense = expenses.Where(e => e.IdExpense == ID).FirstOrDefault();

            if (expense == null)
            {
                return NotFound();
            }

            expense.Active = false;
            return Ok(await _expenseService.UpdateExpense(expense));
        }
    }
}
