namespace BillLedger.Application.Billing.Commands;

public sealed class PayInvoiceCommandHandler(
    IInvoiceRepository repository,
    ILogger<PayInvoiceCommandHandler> logger)
    : ICommandHandler<PayInvoiceCommand, Result>
{
    public async Task<Result> HandleAsync(PayInvoiceCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting invoice payment. InvoiceId: {InvoiceId}, PaidAt: {PaidAt}",
            command.Id,
            command.Request.PaidAt);

        var invoice = await repository.GetByIdAsync(command.Id, cancellationToken);

        if (invoice is null)
        {
            logger.LogWarning(
                "Invoice payment failed because the invoice was not found. InvoiceId: {InvoiceId}",
                command.Id);

            return Result.Failure(BillingErrors.NotFound);
        }

        invoice.MarkAsPaid(command.Request.PaidAt);

        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Invoice successfully paid. InvoiceId: {InvoiceId}",
            command.Id);

        return Result.Success();
    }
}