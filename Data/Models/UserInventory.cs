using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventarioApi.Data.Models
{
    public class UserInventory
    {
        public int ID_USER_INVENTORY { get; set; }
        [ForeignKey("User")]
        public int USER { get; set; }
        [ForeignKey("Product")]
        public int INVENTORY { get; set; }
        public DateTimeOffset DATE { get; set; }
        public UserInventoryType TYPE { get; set; }
    }
}
