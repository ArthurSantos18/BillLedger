namespace BillLedger.Application.Customers.Requests;

public sealed class GetCustomersRequest : PageFilter
{
    public GetCustomersRequest() => PageFilter = new PageFilter { Page = 1, PageSize = 60 };

    [Description("Os parâmetros de paginação da consulta.")]
    public PageFilter PageFilter { get; set; }

    [Description("Filtra pelo nome do cliente.")]
    public string? Name { get; set; }

    [Description("Filtra pelo e-mail exato.")]
    public string? Email { get; set; }

    [Description("Filtra pelo CPF exato.")]
    public string? Cpf { get; set; }

    [Description("Filtra pelo status de ativação.")]
    public bool? IsActive { get; set; }
}
