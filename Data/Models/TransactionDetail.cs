using System.ComponentModel.DataAnnotations.Schema;
namespace inventarioApi.Data.Models
{
    public class TransactionDetail
    {
        public int ID_TRANSACTION_DETAIL { get; set; }
        public int QUANTITY { get; set; }
        [ForeignKey("Presentation")]
        public int PRESENTATION { get; set; }
        [ForeignKey("Tramsaction")]
        public int TRANSACTION { get; set; }
    }
}
