namespace BillLedger.Application.Billing.Errors;

public static class BillingErrors
{
    public static readonly Error NotFound = new(404, "Fatura não encontrada.");
    public static readonly Error CustomerNotFound = new(404, "Cliente não encontrado para vincular à fatura.");
}
