namespace BillLedger.Application.Customers.Commands;

public sealed class RenameCustomerCommandHandler(
    ICustomerRepository repository,
    ILogger<RenameCustomerCommandHandler> logger)
    : ICommandHandler<RenameCustomerCommand, Result<CustomerResponse>>
{
    public async Task<Result<CustomerResponse>> HandleAsync(RenameCustomerCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting customer rename. CustomerId: {CustomerId}",
            command.Id);

        var customer = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (customer is null)
        {
            logger.LogWarning(
                "Customer not found. Cannot rename customer. CustomerId: {CustomerId}",
                command.Id);

            return Result<CustomerResponse>.Failure(CustomerErrors.NotFound);
        }

        customer.Rename(command.Request.Name);

        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Customer renamed successfully. CustomerId: {CustomerId}",
            command.Id);

        return Result<CustomerResponse>.Success(customer.Adapt<CustomerResponse>());
    }
}