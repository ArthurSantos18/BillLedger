namespace BillLedger.Infrastructure.Services;

public sealed class EmailService(ILogger<EmailService> logger) : IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken)
    {
        logger.LogInformation("=== EMAIL SENT ===");
        logger.LogInformation("To: {To}", to);
        logger.LogInformation("Subject: {Subject}", subject);
        logger.LogInformation("Body: {Body}", body);
        logger.LogInformation("==================");

        return Task.CompletedTask;
    }
}
