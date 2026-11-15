namespace BillLedger.Infrastructure.Data.Configurations;

public class CustomerConfiguration : BaseConfiguration<Customer>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Email)
            .HasConversion(new EmailConverter())
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.Cpf)
            .HasConversion(new CpfConverter())
            .IsRequired()
            .HasMaxLength(11);
    }
}
