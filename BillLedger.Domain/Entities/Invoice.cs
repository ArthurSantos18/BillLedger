namespace BillLedger.Domain.Entities;

public sealed class Invoice : Entity<Invoice>
{
    public Guid CustomerId { get; private set; }
    public string Number { get; private set; }
    public Money Total { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    private Invoice(Guid customerId, string number, Money total, DateTime dueDate)
    {
        SetId(Guid.NewGuid());
        SetCreatedAt(DateTime.UtcNow);
        CustomerId = customerId;
        Number = number;
        Total = total;
        DueDate = dueDate;
        Status = InvoiceStatus.Draft;
    }

    public static Invoice Create(Guid customerId, string number, Money total, DateTime dueDate)
    {
        if (customerId == Guid.Empty)
        {
            throw new DomainException("Cliente é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(number))
        {
            throw new DomainException("Número da fatura é obrigatório.");
        }

        if (number.Length > 50)
        {
            throw new DomainException("Número da fatura não pode exceder 50 caracteres.");
        }

        if (total is null)
        {
            throw new DomainException("Valor da fatura é obrigatório.");
        }

        if (total.Amount <= 0)
        {
            throw new DomainException("Valor da fatura deve ser maior que zero.");
        }

        if (dueDate.Date < DateTime.UtcNow.Date)
        {
            throw new DomainException("Data de vencimento não pode ser no passado.");
        }

        return new Invoice(customerId, number.Trim(), total, dueDate);
    }

    public void Issue()
    {
        if (Status != InvoiceStatus.Draft)
        {
            throw new StateConflictException("Apenas faturas em rascunho podem ser emitidas.");
        }

        Status = InvoiceStatus.Issued;

        Touch();
        RaiseDomainEvent(new InvoiceIssuedDomainEvent(Id, CustomerId, Total.Amount, Total.Currency.Code));
    }

    public void MarkAsPaid(DateTime? paidAt = null)
    {
        if (Status == InvoiceStatus.Draft)
        {
            throw new StateConflictException("Fatura em rascunho não pode ser paga sem ser emitida.");
        }

        if (Status == InvoiceStatus.Paid)
        {
            throw new StateConflictException("Fatura já está paga.");
        }

        if (Status == InvoiceStatus.Cancelled)
        {
            throw new StateConflictException("Fatura cancelada não pode ser paga.");
        }

        PaidAt = paidAt ?? DateTime.UtcNow;
        Status = InvoiceStatus.Paid;

        Touch();
        RaiseDomainEvent(new InvoicePaidDomainEvent(Id, CustomerId, Total.Amount, Total.Currency.Code, PaidAt.Value));
    }

    public void Cancel(DateTime? cancelledAt = null)
    {
        if (Status == InvoiceStatus.Paid)
        {
            throw new StateConflictException("Fatura paga não pode ser cancelada.");
        }
            
        if (Status == InvoiceStatus.Cancelled)
        {
            throw new StateConflictException("Fatura já está cancelada.");
        }

        CancelledAt = cancelledAt ?? DateTime.UtcNow;
        Status = InvoiceStatus.Cancelled;

        Touch();
        RaiseDomainEvent(new InvoiceCancelledDomainEvent(Id, CancelledAt.Value));
    }
}
