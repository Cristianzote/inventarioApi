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
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Inventory>> GetProducts()
        {
            var products = await _productService.GetProducts();

            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("{ID_PRODUCT}")]
        [AllowAnonymous]
        public async Task<ActionResult<Inventory>> GetProduct(int ID_PRODUCT)
        {
            var product = await _productService.GetProduct(ID_PRODUCT);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        //POST
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateProduct([FromBody] Product PRODUCT)
        {
            var product = await _productService.CreateProduct(PRODUCT);
            return Ok(product);
        }
        //UPDATE
        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateProduct([FromBody] Product PRODUCT)
        {
            var product = await _productService.UpdateProduct(PRODUCT);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
