namespace BillLedger.Application.Billing.Requests;

public sealed class GetInvoicesRequest : PageFilter
{
    public GetInvoicesRequest() => PageFilter = new PageFilter { Page = 1, PageSize = 60 };

    [Description("Os parâmetros de paginação da consulta.")]
    public PageFilter PageFilter { get; set; }

    [Description("O identificador do cliente utilizado para filtrar as faturas.")]
    public Guid? CustomerId { get; set; }

    [Description("O status utilizado para filtrar as faturas.")]
    public InvoiceStatus? Status { get; set; }

    [Description("A data mínima de vencimento das faturas.")]
    public DateTime? MinDueDate { get; set; }

    [Description("A data máxima de vencimento das faturas.")]
    public DateTime? MaxDueDate { get; set; }

    [Description("O valor total mínimo das faturas.")]
    public decimal? MinTotalAmount { get; set; }

    [Description("O valor total máximo das faturas.")]
    public decimal? MaxTotalAmount { get; set; }

    [Description("A data mínima de criação das faturas.")]
    public DateTime? MinCreatedAt { get; set; }

    [Description("A data máxima de criação das faturas.")]
    public DateTime? MaxCreatedAt { get; set; }
}
