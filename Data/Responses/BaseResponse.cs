namespace inventarioApi.Data.Responses
{
    public class BaseResponse
    {
        public int Code { get; set; }
        public required object Data { get; set; }
        public required string Message { get; set; }
    }
}
