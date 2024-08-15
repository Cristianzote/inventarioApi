namespace inventarioApi.Data.Models
{
    public class EmailConfig
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int PortDebug { get; set; }
        public int PortResease { get; set; }
    }
}
