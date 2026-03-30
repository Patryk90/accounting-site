using AccountingPortfolio.Controllers;
using System.Threading.Channels;

namespace AccountingPortfolio.Services;

public record EmailMessage(string To, string Subject, string Body);

public class EmailQueueService(IEmailService emailService, ILogger<EmailQueueService> logger) : BackgroundService
{
    private readonly Channel<EmailMessage> _emailChannel = Channel.CreateUnbounded<EmailMessage>();

    public async Task EnqueueEmailAsync(string to, string subject, string body)
    {
        try
        {
            await _emailChannel.Writer.WriteAsync(new EmailMessage(to, subject, body));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var email in _emailChannel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await emailService.SendEmailAsync(email.To, email.Subject, email.Body);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send email to {To}", email.To);
            }
        }
    }
}