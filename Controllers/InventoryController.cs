using inventarioApi.Data.Services;
using Microsoft.AspNetCore.Mvc;
using inventarioApi.Data.Models;
using Microsoft.AspNetCore.Authorization;
using inventarioApi.Data.DTO;

namespace inventarioApi.Controllers
{
    [ApiController]
    [Route("/api/v1/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        //GET
        [HttpGet("ok")]
        [AllowAnonymous]
        public ActionResult Get()
        {
            return Ok("Funciona");
        }

        [HttpGet("{ID_INVENTORY}")]
        [AllowAnonymous]
        public async Task<ActionResult<Inventory>> GetInventory(int ID_INVENTORY)
        {
            var inventory = await _inventoryService.GetInventoryById(ID_INVENTORY);

            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);
        }

        [HttpGet("all")]
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
        }
    }
    
}
