using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace inventarioApi.Data.Models
{
    public class Presentation
    {
        public int IdPresentation { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int PriceRetail { get; set; }
        public int PriceRetailCover { get; set; }
        public float PriceIncome { get; set; }
        public float PriceOutput { get; set; }
        public float PriceOutputCover { get; set; }
        public bool HasRetail { get; set; }
        public int Stock { get; set; }
        public DateTimeOffset Date { get; set; }
        [ForeignKey("Product")]
        public int Product { get; set; }
        [JsonIgnore]
        public Product? Products { get; set; }
        public Presentation GetPresentation(int id, List<Product> products)
        {
            foreach (var product in products)
            {
                var presentation = product.Presentations.FirstOrDefault(p => p.IdPresentation == id);
                if (presentation != null)
                {
                    return presentation;
                }
            }
            return null;
        }
    }
}
