﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace inventarioApi.Data.Models
{
    public class TransactionDetail
    {
        public int IdTransactionDetail { get; set; }
        public int Quantity { get; set; }
        public bool Detail { get; set; }
        public int Discounting { get; set; }
        [ForeignKey("Presentation")]
        public int Presentation { get; set; }
        [ForeignKey("Transaction")]
        public int Transaction { get; set; }
        [JsonIgnore]
        public Transaction? Transactions { get; set; }
    }
}
