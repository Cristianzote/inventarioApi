using inventarioApi.Data.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace inventarioApi.Data.Services
{
    public class Message : IMessage
    {
        private readonly RazorViewToStringRenderer _razorViewToStringRenderer;
        public EmailConfig _emailConfig { get; }
        public Message(IOptions<EmailConfig> emailConfig, RazorViewToStringRenderer razorViewToStringRenderer) 
        {
            _emailConfig = emailConfig.Value;
            _razorViewToStringRenderer = razorViewToStringRenderer;
        }
        public async Task<bool> SendEmail(string subject, string viewName, Transaction model, string to)
        {
            var fromEmail = _emailConfig.Username;
            var password = _emailConfig.Password;
            var port = _emailConfig.PortDebug;

            try
            {
                var body = await _razorViewToStringRenderer.RenderViewToStringAsync(viewName, model);

                var message = new MailMessage();
                message.From = new MailAddress(fromEmail);
                message.To.Add(new MailAddress(to));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                //message.Attachments.Add(new Attachment(1, 2));

                var smtpClient = new SmtpClient("smtp.gmail.com") 
                { 
                    Credentials = new NetworkCredential(fromEmail, password),
                    Port = port,
                    EnableSsl = true
                };

                smtpClient.Send(message);

                return true;
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
                throw new Exception($"Error al enviar correo (SMTP): {smtpEx.Message}", smtpEx);
            }
            catch (Exception e)
            {
                Console.WriteLine($"General Error: {e.Message}");
                throw new Exception($"Error al enviar correo: {e.Message}", e);
            }
        }
    }
}