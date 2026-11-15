namespace BillLedger.Infrastructure.Data.Configurations;

public class InvoiceConfiguration : BaseConfiguration<Invoice>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.Property(i => i.Number)
            .IsRequired()
            .HasMaxLength(50);

        builder.ComplexProperty(i => i.Total, total =>
        {
            total.Property(m => m.Amount)
                .HasColumnName("TotalAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            total.ComplexProperty(m => m.Currency, currency =>
            {
                currency.Property(c => c.Code)
                    .HasColumnName("TotalCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });
        });

        builder.Property(i => i.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);
    }
}
