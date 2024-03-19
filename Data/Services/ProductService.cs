using inventarioApi.Data.Models;
using InventarioApi;
using Microsoft.EntityFrameworkCore;

namespace inventarioApi.Data.Services
{
    public class ProductService
    {
        private readonly InventarioContext _context;
        public ProductService(InventarioContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProducts(int ID_INVENTORY)
        {
            var result = await _context.Products
            .Include(p => p.Presentations)
            .Where(p => p.INVENTORY == ID_INVENTORY)
            .Where(p => p.INVENTORY == ID_INVENTORY)
            .ToListAsync();

            if (result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("Null value");
            }
        }
        public async Task<Product> GetProduct(int ID_INVENTORY, int ID_PRODUCT)
        {
            var result = await _context.Products
            .Include(p => p.Presentations)
            .Where(p => p.INVENTORY == ID_INVENTORY)
            .Where(p => p.ID_PRODUCT == ID_PRODUCT)
            .FirstAsync();

            if (result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("Null value");
            }
        }

        //POST
        public async Task<Product> CreateProduct(Product PRODUCT, int ID_INVENTORY)
        {

            var ProductEntity = new Product
            {
                NAME = PRODUCT.NAME,
                DESCRIPTION = PRODUCT.DESCRIPTION,
                IMAGE = PRODUCT.IMAGE,
                DATE = DateTimeOffset.UtcNow,
                INVENTORY = ID_INVENTORY,
                Presentations = PRODUCT.Presentations
            };

            await _context.Products.AddAsync(ProductEntity);

            try
            {
                await _context.SaveChangesAsync();
                int newProductId = ProductEntity.ID_PRODUCT;

                //Create product presentations
                foreach (Presentation presentations in ProductEntity.Presentations)
                {
                    var PresentationEntity = new Presentation
                    {
                        NAME = presentations.NAME,
                        DESCRIPTION = presentations.DESCRIPTION,
                        QUANTITY = presentations.QUANTITY,
                        PRICE_INCOME = presentations.PRICE_INCOME,
                        PRICE_OUTPUT = presentations.PRICE_OUTPUT,
                        STOCK = presentations.STOCK,
                        RETAIL_STOCK = presentations.RETAIL_STOCK,
                        RETAIL_STOCK_RATIO = presentations.RETAIL_STOCK_RATIO,
                        DATE = DateTimeOffset.UtcNow,
                        //PRODUCT = newProductId
                    };

                    await _context.Presentations.AddAsync(presentations);
                }

                //var createUserInventory = await _context.UserInventories.AddAsync(userInventoryEntity);
                //await _context.SaveChangesAsync();
                return ProductEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
