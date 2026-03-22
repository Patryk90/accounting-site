
using System.Net;
using System.Net.Mail;

namespace AccountingPortfolio.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
    {
        try
        {
            var fromEmail = configuration["EmailSettings:From"];
            var smtpServer = configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(configuration["EmailSettings:Port"]);
            var smtpUser = configuration["EmailSettings:Username"];
            var smtpPass = configuration["EmailSettings:Password"];

            using var message = new MailMessage();
            message.From = new MailAddress(fromEmail);
            message.To.Add(new MailAddress(toEmail));
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true;

            using var client = new SmtpClient(smtpServer, smtpPort);
            client.Credentials = new NetworkCredential(smtpUser, smtpPass);
            client.EnableSsl = true;

            await client.SendMailAsync(message);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Smtp Send Failed: {e.Message}");
        }        
    }
}