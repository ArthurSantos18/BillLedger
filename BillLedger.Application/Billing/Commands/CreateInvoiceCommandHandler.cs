namespace BillLedger.Application.Billing.Commands;

public sealed class CreateInvoiceCommandHandler(
    IInvoiceRepository invoiceRepository,
    ICustomerRepository customerRepository,
    ILogger<CreateInvoiceCommandHandler> logger)
    : ICommandHandler<CreateInvoiceCommand, Result<InvoiceResponse>>
{
    public async Task<Result<InvoiceResponse>> HandleAsync(CreateInvoiceCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting invoice creation. CustomerId: {CustomerId}",
            command.Request.CustomerId);

        var customer = await customerRepository.GetByIdAsync(command.Request.CustomerId, cancellationToken);

        if (customer is null)
        {
            logger.LogWarning(
                "Invoice creation failed because the customer was not found. CustomerId: {CustomerId}",
                command.Request.CustomerId);

            return Result<InvoiceResponse>.Failure(BillingErrors.CustomerNotFound);
        }

        var currency = Currency.FromCode(command.Request.CurrencyCode);
        var total = Money.Create(command.Request.Amount, currency);

        var invoice = Invoice.Create(command.Request.CustomerId, command.Request.Number, total, command.Request.DueDate);

        await invoiceRepository.AddAsync(invoice, cancellationToken);
        await invoiceRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Invoice successfully created. InvoiceId: {InvoiceId}, CustomerId: {CustomerId}",
            invoice.Id,
            command.Request.CustomerId);

        return Result<InvoiceResponse>.Success(invoice.Adapt<InvoiceResponse>());
    }
}