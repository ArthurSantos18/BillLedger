namespace BillLedger.Application.Customers.Requests;

public sealed class RenameCustomerRequest
{
    [Description("O nome completo do cliente.")]
    public string Name { get; set; } = string.Empty;
}
