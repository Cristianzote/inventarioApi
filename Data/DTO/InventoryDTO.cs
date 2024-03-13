namespace inventarioApi.Data.DTO
{
    public class InventoryDTO
    {
        public int ID_INVENTORY { get; }
        public string TITLE { get; set; }
        public string DESCRIPTION { get; set; }
        public string IMAGE { get; set; }
        public DateTimeOffset DATE { get; }
    }
}
