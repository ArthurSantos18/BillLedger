namespace BillLedger.Application.Billing.Commands;

public sealed class CancelInvoiceCommandHandler(
    IInvoiceRepository repository,
    ILogger<CancelInvoiceCommandHandler> logger)
    : ICommandHandler<CancelInvoiceCommand, Result>
{
    public async Task<Result> HandleAsync(CancelInvoiceCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting invoice cancellation. InvoiceId: {InvoiceId}",
            command.Id);

        var invoice = await repository.GetByIdAsync(command.Id, cancellationToken);

        if (invoice is null)
        {
            logger.LogWarning(
                "Invoice cancellation failed because the invoice was not found. InvoiceId: {InvoiceId}",
                command.Id);

            return Result.Failure(BillingErrors.NotFound);
        }

        invoice.Cancel();

        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Invoice successfully cancelled. InvoiceId: {InvoiceId}",
            command.Id);

        return Result.Success();
    }
}