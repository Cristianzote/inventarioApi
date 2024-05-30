using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventarioApi.Data.Models
{
    public class Inventory
    {
        public int IdInventory { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public DateTimeOffset Date { get; set; }

        //PK
        //public ICollection<UserInventory>? UserInventories { get; set; }
        //public ICollection<Product>? Products { get; set; }
    }
}