using System.ComponentModel.DataAnnotations.Schema;

namespace inventarioApi.Data.Models
{
    public class Product
    {
        public int IdProduct { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int Category { get; set; }
        public required string Image { get; set; }
        public DateTimeOffset Date { get; set; }
        //[ForeignKey("Inventory")]
        //public int INVENTORY { get; set; }

        //PK
        public ICollection<Presentation> Presentations { get; set; }
        //FK

        //[ForeignKey("Inventory")]
        //public Inventory? INVENTORY { get; set; }

        public Product GetProduct(int id, List<Product> product)
        {
            return product.FirstOrDefault(p => id == p.IdProduct);
        }
    }
}
 