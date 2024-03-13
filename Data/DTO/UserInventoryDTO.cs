namespace inventarioApi.Data.DTO
{
    public class UserInventoryDTO
    {
        public int ID_USER_INVENTORY { get; set; }
        public int USER { get; set; }
        public int INVENTORY { get; set; }
        public DateTimeOffset DATE { get; set; }
        public UserInventoryType TYPE { get; set; }
    }
}

public enum UserInventoryType
{
    OWNER = 1,
    INVETED = 2
}