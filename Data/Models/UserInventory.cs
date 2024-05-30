using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventarioApi.Data.Models
{
    public class UserInventory
    {
        public int IdUserInventory { get; set; }
        [ForeignKey("User")]
        public int User { get; set; }
        [ForeignKey("Inventory")]
        public int Inventory { get; set; }
        public DateTimeOffset Date { get; set; }
        public UserInventoryType Type { get; set; }
    }
}
