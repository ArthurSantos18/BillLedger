namespace BillLedger.Application.Notifications.Events;

public sealed class SendInvoiceCancelledEmailHandler(
    IEmailService emailService,
    ILogger<SendInvoiceCancelledEmailHandler> logger)
    : IEventHandler<InvoiceCancelledDomainEvent>
{
    public async Task HandleAsync(InvoiceCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Invoice cancelled. Notifying customer. InvoiceId: {InvoiceId}",
            domainEvent.InvoiceId);

        await emailService.SendEmailAsync(
            to: "cliente@exemplo.com",
            subject: "Fatura Cancelada",
            body: $"Sua fatura foi cancelada em {domainEvent.CancelledAt:dd/MM/yyyy}. Por favor, desconsidere o boleto anterior.",
            cancellationToken: cancellationToken);
    }
}
