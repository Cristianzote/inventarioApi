using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventarioApi.Data.Models
{
    public class User
    {
        public int IdUser { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public Guid UUID { get; set; }
        public DateTimeOffset Date { get; set; }

        //PK
        //public ICollection<UserInventory>? UserInventories { get; set; }
    }
}
