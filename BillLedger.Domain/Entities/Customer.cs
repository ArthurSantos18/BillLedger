namespace BillLedger.Domain.Entities;

public sealed class Customer : Entity<Customer>
{
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Cpf Cpf { get; private set; }

    private Customer()
    {
        Name = string.Empty;
        Email = Email.Create("placeholder@example.com");
        Cpf = Cpf.Create("11144477735");
    }

    private Customer(string name, Email email, Cpf cpf)
    {
        SetId(Guid.NewGuid());
        SetCreatedAt(DateTime.UtcNow);
        Name = name;
        Email = email;
        Cpf = cpf;
    }

    public static Customer Create(string name, Email email, Cpf cpf)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Nome é obrigatório.");
        }

        if (name.Length > 200)
        {
            throw new DomainException("Nome não pode exceder 200 caracteres.");
        }

        if (email is null)
        {
            throw new DomainException("E-mail é obrigatório.");
        }

        if (cpf is null)
        {
            throw new DomainException("CPF é obrigatório.");
        }

        var customer = new Customer(name.Trim(), email, cpf);

        customer.RaiseDomainEvent(new CustomerCreatedDomainEvent(customer.Id, email.Value, cpf.Value));

        return customer;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new DomainException("Nome é obrigatório.");
        }

        if (newName.Length > 200)
        {
            throw new DomainException("Nome não pode exceder 200 caracteres.");
        }

        Name = newName.Trim();
        Touch();
    }

    public void ChangeEmail(Email newEmail)
    {
        if (newEmail is null)
        {
            throw new DomainException("E-mail é obrigatório.");
        }

        Email = newEmail;
        Touch();
    }
}
