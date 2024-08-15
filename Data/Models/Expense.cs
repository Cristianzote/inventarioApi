namespace inventarioApi.Data.Models
{
    public class Expense
    {
        public int IdExpences { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Value { get; set; }
        public ExpenceType Type { get; set; }
        public double Multiplier { get; set; }
        public bool Active { get; set; }
    }

    public enum ExpenceType
    {
        OCCASIONAL = 1,
        RECURRENT = 2,
    }
}
