namespace inventarioApi.Data.DTO
{
    public record SendEmailRequest(string Subject, string Body, string To);
}
