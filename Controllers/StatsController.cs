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

        /*[HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetAllInventories()
        {
            var inventories = await _inventoryService.GetInventoriesAsync();
            return Ok(inventories);
        }
        
        [HttpGet("userInventories/{ID_USER}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UserInventory>>> GetUserInventories(int ID_USER)
        {
            var inventories = await _inventoryService.GetUserInventories(ID_USER);
            return Ok(inventories);
        }

        //POST
        [HttpPost("createInventory/{ID_USER}")]
        [AllowAnonymous]
        public async Task<ActionResult> CreateInventory([FromBody] Inventory INVENTORY, int ID_USER)
        {
            var userInventories = await _inventoryService.CreateInventory(INVENTORY, ID_USER);
            return Ok(userInventories);
        }*/
    }
    
}