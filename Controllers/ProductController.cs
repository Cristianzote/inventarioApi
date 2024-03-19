using inventarioApi.Data.Services;
using Microsoft.AspNetCore.Mvc;
using inventarioApi.Data.Models;
using Microsoft.AspNetCore.Authorization;
using inventarioApi.Data.DTO;

namespace inventarioApi.Controllers
{
    [ApiController]
    [Route("/api/v1/product")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        //GET
        [HttpGet("{ID_INVENTORY}")]
        [AllowAnonymous]
        public async Task<ActionResult<Inventory>> GetProducts(int ID_INVENTORY)
        {
            var products = await _productService.GetProducts(ID_INVENTORY);

            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("{ID_INVENTORY}/{ID_PRODUCT}")]
        [AllowAnonymous]
        public async Task<ActionResult<Inventory>> GetProduct(int ID_INVENTORY, int ID_PRODUCT)
        {
            var product = await _productService.GetProduct(ID_INVENTORY, ID_PRODUCT);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        //POST
        [HttpPost("{ID_INVENTORY}")]
        [AllowAnonymous]
        public async Task<ActionResult> CreateProduct([FromBody] Product PRODUCT, int ID_INVENTORY)
        {
            var product = await _productService.CreateProduct(PRODUCT, ID_INVENTORY);
            return Ok(product);
        }
    }
}
