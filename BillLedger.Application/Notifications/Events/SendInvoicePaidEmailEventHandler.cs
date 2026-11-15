namespace BillLedger.Application.Notifications.Events;

public sealed class SendInvoicePaidEmailEventHandler(
    IEmailService emailService,
    ILogger<SendInvoicePaidEmailEventHandler> logger)
    : IEventHandler<InvoicePaidDomainEvent>
{
    public async Task HandleAsync(InvoicePaidDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Invoice paid. Sending payment receipt. InvoiceId: {InvoiceId}",
            domainEvent.InvoiceId);

        await emailService.SendEmailAsync(
            to: "cliente@exemplo.com",
            subject: "Pagamento Confirmado!",
            body: $"Sua fatura de {domainEvent.TotalAmount} {domainEvent.CurrencyCode} foi paga com sucesso em {domainEvent.PaidAt:dd/MM/yyyy}.",
            cancellationToken: cancellationToken);
    }
}