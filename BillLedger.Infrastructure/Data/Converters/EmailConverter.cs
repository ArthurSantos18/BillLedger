namespace BillLedger.Infrastructure.Data.Converters;

public sealed class EmailConverter() : ValueConverter<Email, string>(
    email => email.Value,
    value => Email.Create(value));
