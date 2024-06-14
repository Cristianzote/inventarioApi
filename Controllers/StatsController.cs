using inventarioApi.Data.Services;
using Microsoft.AspNetCore.Mvc;
using inventarioApi.Data.Models;
using Microsoft.AspNetCore.Authorization;
using inventarioApi.Data.DTO;

namespace inventarioApi.Controllers
{
    [ApiController]
    [Route("/api/v1/stats")]
    public class StatsController : ControllerBase
    {
        private readonly StatsService _statsService;

        public StatsController(StatsService statsService)
        {
            _statsService = statsService;
        }

        //GET
        [HttpGet("sales")]
        public async Task<IActionResult> GetSalesAndInflowsStatistics()
        {
            var stats = await _statsService.GetSalesAndInflowsStatistics();
            return Ok(stats);
        }

        [HttpGet("coverage")]
        public async Task<IActionResult> GetStockCoverage()
        {
            var stats = await _statsService.GetStockCoverage();
            return Ok(stats);
        }
    }
    
}