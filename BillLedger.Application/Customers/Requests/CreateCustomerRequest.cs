namespace BillLedger.Application.Customers.Requests;

public sealed class CreateCustomerRequest
{
    [Description("O nome completo do cliente.")]
    public string Name { get; set; } = string.Empty;

    [Description("O e-mail do cliente.")]
    public string Email { get; set; } = string.Empty;

    [Description("O CPF do cliente (apenas números ou formatado).")]
    public string Cpf { get; set; } = string.Empty;
}
