using System.ComponentModel.DataAnnotations.Schema;
namespace inventarioApi.Data.Models
{
    public class Transaction
    {
        public int ID_TRANSACTION { get; set; }
        public float VALUE { get; set; }
        public DateTimeOffset DATE { get; set; }
        [ForeignKey("Inventory")]
        public int INVENTORY { get; set; }
        public TransactionType TYPE { get; set; }
    }

    public enum TransactionType
    {
        INCOME = 1,
        OUTPUT = 2
    }
}
