namespace BillLedger.Application.Billing.Commands;

public sealed class IssueInvoiceCommandHandler(
    IInvoiceRepository repository,
    ILogger<IssueInvoiceCommandHandler> logger)
    : ICommandHandler<IssueInvoiceCommand, Result<InvoiceResponse>>
{
    public async Task<Result<InvoiceResponse>> HandleAsync(IssueInvoiceCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting invoice issuance. InvoiceId: {InvoiceId}",
            command.Id);

        var invoice = await repository.GetByIdAsync(command.Id, cancellationToken);

        if (invoice is null)
        {
            logger.LogWarning(
                "Invoice issuance failed because the invoice was not found. InvoiceId: {InvoiceId}",
                command.Id);

            return Result<InvoiceResponse>.Failure(BillingErrors.NotFound);
        }

        invoice.Issue();

        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Invoice successfully issued. InvoiceId: {InvoiceId}",
            command.Id);

        return Result<InvoiceResponse>.Success(invoice.Adapt<InvoiceResponse>());
    }
}
