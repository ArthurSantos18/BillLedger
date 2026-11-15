namespace BillLedger.Application.Customers.Commands;

public sealed class CreateCustomerCommandHandler(
    ICustomerRepository repository,
    ILogger<CreateCustomerCommandHandler> logger)
    : ICommandHandler<CreateCustomerCommand, Result<CustomerResponse>>
{
    public async Task<Result<CustomerResponse>> HandleAsync(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting customer creation.");

        var email = Email.Create(command.Request.Email);
        var cpf = Cpf.Create(command.Request.Cpf);

        if (await repository.GetByCpfAsync(cpf, cancellationToken) is not null)
        {
            logger.LogWarning("Customer creation failed because the CPF is already in use.");

            return Result<CustomerResponse>.Failure(CustomerErrors.CpfAlreadyInUse);
        }

        if (await repository.GetByEmailAsync(email, cancellationToken) is not null)
        {
            logger.LogWarning("Customer creation failed because the email is already in use.");

            return Result<CustomerResponse>.Failure(CustomerErrors.EmailAlreadyInUse);
        }

        var customer = Customer.Create(command.Request.Name, email, cpf);

        await repository.AddAsync(customer, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        var response = customer.Adapt<CustomerResponse>();

        logger.LogInformation(
            "Customer successfully created. CustomerId: {CustomerId}",
            customer.Id);

        return Result<CustomerResponse>.Success(response);
    }
}