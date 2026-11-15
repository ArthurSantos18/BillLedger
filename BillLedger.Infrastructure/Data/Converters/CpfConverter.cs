namespace BillLedger.Infrastructure.Data.Converters;

public sealed class CpfConverter() : ValueConverter<Cpf, string>(
    cpf => cpf.Value,
    value => Cpf.Create(value));
