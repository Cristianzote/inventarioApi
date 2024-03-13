using System.ComponentModel.DataAnnotations.Schema;

namespace inventarioApi.Data.Models
{
    public class Presentation
    {
        public int ID_PRESENTATION { get; set; }
        public required string NAME { get; set; }
        public required string DESCRIPTION { get; set; }
        public int QUANTITY { get; set; }
        public float PRICE_INCOME { get; set; }
        public float PRICE_OUTPUT { get; set; }
        public int STOCK { get; set; }
        public int RETAIL_STOCK { get; set; }
        public int RETAIL_STOCK_RATIO { get; set; }
        public DateTimeOffset DATE { get; set; }
        [ForeignKey("Product")]
        public int PRODUCT { get; set; }
    }
}
