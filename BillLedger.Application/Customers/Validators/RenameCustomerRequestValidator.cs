namespace BillLedger.Application.Customers.Validators;

public sealed class RenameCustomerRequestValidator : AbstractValidator<RenameCustomerRequest>
{
    public RenameCustomerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");
    }
}
