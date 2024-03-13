using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventarioApi.Data.Models
{
    public class User
    {
        public int ID_USER { get; set; }
        public required string NAME { get; set; }
        public required string EMAIL { get; set; }
        public Guid UUID { get; set; }
        public DateTimeOffset DATE { get; set; }

        //PK
        //public ICollection<UserInventory>? UserInventories { get; set; }
    }
}
