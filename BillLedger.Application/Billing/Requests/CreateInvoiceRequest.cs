namespace BillLedger.Application.Billing.Requests;

public sealed class CreateInvoiceRequest
{
    [Description("O identificador do cliente associado à fatura.")]
    public Guid CustomerId { get; set; }

    [Description("O número de identificação da fatura.")]
    public string Number { get; set; } = string.Empty;

    [Description("O valor monetário total da fatura.")]
    public decimal Amount { get; set; }

    [Description("O código ISO 4217 da moeda utilizada na fatura. O valor padrão é BRL.")]
    public string CurrencyCode { get; set; } = "BRL";

    [Description("A data de vencimento da fatura.")]
    public DateTime DueDate { get; set; }
}
