using inventarioApi.Data.Services;
using Microsoft.AspNetCore.Mvc;
using inventarioApi.Data.Models;
using Microsoft.AspNetCore.Authorization;
using inventarioApi.Data.DTO;

namespace inventarioApi.Controllers
{

    [ApiController]
    [Route("/api/v1/monthly")]
    public class MonthlyRegisterController : ControllerBase
    {
        private readonly MonthlyRegisterService _monthlyRegisterService;

        public MonthlyRegisterController(MonthlyRegisterService monthlyRegisterService)
        {
            _monthlyRegisterService = monthlyRegisterService;
        }

        //GET
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Expense>> GetExpenses()
        {
            var monthlyRegisters = await _monthlyRegisterService.GetMonthlyRegisters();

            if (monthlyRegisters == null)
            {
                return NotFound();
            }
            return Ok(monthlyRegisters);
        }
    }
}
