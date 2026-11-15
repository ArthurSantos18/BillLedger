namespace BillLedger.Application.Customers.Commands;

public sealed class DeleteCustomerCommandHandler(
    ICustomerRepository repository,
    ILogger<DeleteCustomerCommandHandler> logger)
    : ICommandHandler<DeleteCustomerCommand, Result>
{
    public async Task<Result> HandleAsync(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Starting customer deletion. CustomerId: {CustomerId}",
            command.Id);

        var customer = await repository.GetByIdAsync(command.Id, cancellationToken);

        if (customer is null)
        {
            logger.LogWarning(
                "Customer not found. Cannot delete customer. CustomerId: {CustomerId}",
                command.Id);

            return Result.Failure(CustomerErrors.NotFound);
        }

        customer.MarkAsDeleted();

        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Customer deleted successfully. CustomerId: {CustomerId}",
            command.Id);

        return Result.Success();
    }
}