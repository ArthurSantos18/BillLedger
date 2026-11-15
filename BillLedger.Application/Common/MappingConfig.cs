namespace BillLedger.Application.Common;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Customer, CustomerResponse>
            .NewConfig()
            .Map(dest => dest.Email, src => src.Email.Value)
            .Map(dest => dest.Cpf, src => src.Cpf.Value);

        TypeAdapterConfig<Invoice, InvoiceResponse>
            .NewConfig()
            .Map(dest => dest.Amount, src => src.Total.Amount)
            .Map(dest => dest.Currency, src => src.Total.Currency.Code);

        TypeAdapterConfig.GlobalSettings
            .NewConfig(typeof(PagedResult<>), typeof(PagedResult<>));
    }
}
