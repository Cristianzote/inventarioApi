using System.ComponentModel.DataAnnotations.Schema;

namespace inventarioApi.Data.Models
{
    public class Product
    {
        public int ID_PRODUCT { get; set; }
        public required string NAME { get; set; }
        public required string DESCRIPTION { get; set; }
        public required string IMAGE { get; set; }
        public DateTimeOffset DATE { get; set; }
        [ForeignKey("Inventory")]
        public int INVENTORY { get; set; }

        //PK
        //public ICollection<Presentation>? Presentations { get; set; }
        //FK

        //[ForeignKey("Inventory")]
        //public Inventory? INVENTORY { get; set; }
    }
}
