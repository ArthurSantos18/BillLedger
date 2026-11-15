namespace BillLedger.Application.Notifications.Events;

public sealed class SendInvoiceIssuedEmailEventHandler(
    IEmailService emailService,
    ILogger<SendInvoiceIssuedEmailEventHandler> logger)
    : IEventHandler<InvoiceIssuedDomainEvent>
{
    public async Task HandleAsync(InvoiceIssuedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Invoice issued. Preparing to send notification email. InvoiceId: {InvoiceId}",
            domainEvent.InvoiceId);

        await emailService.SendEmailAsync(
            to: "cliente@exemplo.com",
            subject: "Sua fatura foi emitida!",
            body: $"Sua fatura no valor de {domainEvent.TotalAmount} {domainEvent.CurrencyCode} foi gerada e já está disponível para pagamento.",
            cancellationToken: cancellationToken);
    }
}