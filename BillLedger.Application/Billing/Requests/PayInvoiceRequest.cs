namespace BillLedger.Application.Billing.Requests;

public sealed class PayInvoiceRequest
{
    [Description("A data e hora em que a fatura foi paga.")]
    public DateTime? PaidAt { get; set; }
}
