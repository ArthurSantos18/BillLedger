namespace BillLedger.Application.Billing.Validators;

public sealed class CreateInvoiceRequestValidator : AbstractValidator<CreateInvoiceRequest>
{
    public CreateInvoiceRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("O ID do cliente é obrigatório.");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("O número da fatura é obrigatório.")
            .MaximumLength(50).WithMessage("O número da fatura deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("O valor da fatura deve ser maior que zero.");

        RuleFor(x => x.CurrencyCode)
            .NotEmpty().WithMessage("A moeda é obrigatória.")
            .Length(3).WithMessage("A moeda deve ter 3 caracteres (ex: BRL, USD).");

        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("A data de vencimento não pode ser no passado.");
    }
}
