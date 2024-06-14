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

        public async Task<List<Product>> GetProducts()
        {
            var result = await _context.Products
            .Include(p => p.Presentations)
            //.Where(p => p.INVENTORY == ID_INVENTORY)
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
        public async Task<Product> GetProduct(int ID_PRODUCT)
        {
            var result = await _context.Products
            .Include(p => p.Presentations)
            //.Where(p => p.INVENTORY == ID_INVENTORY)
            .Where(p => p.IdProduct == ID_PRODUCT)
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
        public async Task<Product> CreateProduct(Product PRODUCT)
        {

            var ProductEntity = new Product
            {
                Name = PRODUCT.Name,
                Description = PRODUCT.Description,
                Image = PRODUCT.Image,
                Category = PRODUCT.Category,
                Date = DateTimeOffset.UtcNow.AddHours(-5),
                //INVENTORY = ID_INVENTORY,
                Presentations = PRODUCT.Presentations
            };

            await _context.Products.AddAsync(ProductEntity);

            try
            {
                await _context.SaveChangesAsync();
                int newProductId = ProductEntity.IdProduct;

                //Create product presentations
                foreach (Presentation presentations in ProductEntity.Presentations)
                {
                    var PresentationEntity = new Presentation
                    {
                        Name = presentations.Name,
                        Description = presentations.Description,
                        PriceRetail = presentations.PriceRetail,
                        PriceRetailCover = presentations.PriceRetailCover,
                        PriceIncome = presentations.PriceIncome,
                        PriceOutput = presentations.PriceOutput,
                        PriceOutputCover = presentations.PriceOutputCover,
                        Stock = presentations.Stock,
                        RetailStock = presentations.RetailStock,
                        RetailStockRatio = presentations.RetailStockRatio,
                        Date = DateTimeOffset.UtcNow.AddHours(-5),
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
        public async Task<Product> UpdateProduct(Product PRODUCT)
        {
            var existingProduct = await _context.Products
            .Include(p => p.Presentations)
            .Where(p => p.IdProduct == PRODUCT.IdProduct)
            .FirstAsync();

            existingProduct.Image = PRODUCT.Image;
            existingProduct.Name = PRODUCT.Name;
            existingProduct.Description = PRODUCT.Description;
            existingProduct.Category = PRODUCT.Category;
            existingProduct.Date = PRODUCT.Date;

            //Update product presentation
            foreach (Presentation presentation in PRODUCT.Presentations)    
            {
                try
                {
                    var presentationUpdated = await _context.Presentations
                    .Where(p => p.IdPresentation == presentation.IdPresentation)
                    .FirstAsync();

                    presentationUpdated = presentation;
                }
                catch(Exception e)
                {
                    await _context.Presentations.AddAsync(presentation);
                }
            }

            _context.Products.Update(existingProduct);
            _context.SaveChanges();

            return existingProduct;
        }
    }
}
