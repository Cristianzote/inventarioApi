using System.ComponentModel.DataAnnotations.Schema;
namespace inventarioApi.Data.Models
{
    public class Transaction
    {
        public int IdTransaction { get; set; }
        public float Value { get; set; }
        public DateTimeOffset Date { get; set; }
        public TransactionType Type { get; set; }
        public ICollection<TransactionDetail> TransactionDetail { get; set; }
    }

    public enum TransactionType
    {
        INCOME = 1,
        OUTPUT = 2,
        DETAIL_OUTPUT = 3
    }
}