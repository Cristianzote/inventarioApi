using System.ComponentModel.DataAnnotations.Schema;

namespace inventarioApi.Data.Models
{
    public class Category
    {
        public int IdCategory { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
