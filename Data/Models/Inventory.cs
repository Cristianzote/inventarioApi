using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventarioApi.Data.Models
{
    public class Inventory
    {
        public int ID_INVENTORY { get; set; }
        public required string TITLE { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? IMAGE { get; set; }
        public DateTimeOffset DATE { get; set; }

        //PK
        //public ICollection<UserInventory>? UserInventories { get; set; }
        //public ICollection<Product>? Products { get; set; }
    }
}