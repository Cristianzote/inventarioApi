namespace inventarioApi.Data.DTO
{
    public class UserDTO
    {
        public int ID_USER { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public Guid UUID { get; set; }
        public DateTimeOffset DATE { get; set; }
    }
}
