using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace inventarioApi.Data.Models
{
    public class Presentation
    {
        public int IdPresentation { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int Quantity { get; set; }
        public int PriceRetail { get; set; }
        public float PriceIncome { get; set; }
        public float PriceOutput { get; set; }
        public int Stock { get; set; }
        public int RetailStock { get; set; }
        public int RetailStockRatio { get; set; }
        public DateTimeOffset Date { get; set; }
        [ForeignKey("Product")]
        public int Product { get; set; }
        [JsonIgnore]
        public Product? Products { get; set; }
    }
}
