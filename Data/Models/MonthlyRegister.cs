namespace inventarioApi.Data.Models
{
    public class MonthlyRegister
    {
        public int IdMonthlyRegister { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public int InitialInventory {  get; set; }
        public int FinalInventory { get; set; }
        public int Purchases { get; set; }
        public int Expenses { get; set; }
    }
}
