using System.ComponentModel.DataAnnotations.Schema;
namespace inventarioApi.Data.Models
{
    public class Transaction
    {
        public int IdTransaction { get; set; }
        public float Value { get; set; }
        public DateTimeOffset Date { get; set; }
        public TransactionType Type { get; set; }
        public Table Table { get; set; }
        public bool Cover { get; set; }
        public ICollection<TransactionDetail> TransactionDetail { get; set; }
    }

    public enum TransactionType
    {
        INCOME = 1,
        OUTPUT = 2,
        DETAIL_OUTPUT = 3
    }
    public enum Table
    {
        SOSSA = 1,
        PIANO = 2,
        VIRGEN = 3,
        RINCON = 4,
        MITAD = 5,
        MORENO = 6,
        BARRA = 7,
        AFUERA = 8,
    }
}