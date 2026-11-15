namespace BillLedger.UnitTests.Domain.Entities;

public sealed class InvoiceTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new DomainCustomization());

    [Fact]
    public void Create_WithValidData_ReturnsDraftInvoice()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var total = _fixture.Create<Money>();
        var dueDate = DateTime.UtcNow.AddDays(30);

        // Act
        var invoice = Invoice.Create(customerId, "INV-001", total, dueDate);

        // Assert
        invoice.Id.Should().NotBe(Guid.Empty);
        invoice.CustomerId.Should().Be(customerId);
        invoice.Number.Should().Be("INV-001");
        invoice.Total.Should().Be(total);
        invoice.Status.Should().Be(InvoiceStatus.Draft);
        invoice.DueDate.Should().Be(dueDate);
        invoice.PaidAt.Should().BeNull();
        invoice.CancelledAt.Should().BeNull();
    }

    [Fact]
    public void Create_WithEmptyCustomerId_ThrowsDomainException()
    {
        // Arrange
        var total = _fixture.Create<Money>();

        // Act
        var act = () => Invoice.Create(Guid.Empty, "INV-001", total, DateTime.UtcNow.AddDays(30));

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*Cliente*obrigatório*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyNumber_ThrowsDomainException(string? invalidNumber)
    {
        // Arrange
        var total = _fixture.Create<Money>();

        // Act
        var act = () => Invoice.Create(Guid.NewGuid(), invalidNumber!, total, DateTime.UtcNow.AddDays(30));

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*Número*obrigatório*");
    }

    [Fact]
    public void Create_WithZeroTotal_ThrowsDomainException()
    {
        // Arrange
        var zeroTotal = Money.Zero(Currency.Brl);

        // Act
        var act = () => Invoice.Create(Guid.NewGuid(), "INV-001", zeroTotal, DateTime.UtcNow.AddDays(30));

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*maior que zero*");
    }

    [Fact]
    public void Create_WithPastDueDate_ThrowsDomainException()
    {
        // Arrange
        var total = _fixture.Create<Money>();
        var pastDue = DateTime.UtcNow.AddDays(-1);

        // Act
        var act = () => Invoice.Create(Guid.NewGuid(), "INV-001", total, pastDue);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*passado*");
    }

    [Fact]
    public void Issue_WhenDraft_TransitionsToIssuedAndRaisesEvent()
    {
        // Arrange
        var invoice = CreateValidInvoice();
        invoice.ClearDomainEvents();

        // Act
        invoice.Issue();

        // Assert
        invoice.Status.Should().Be(InvoiceStatus.Issued);
        invoice.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<InvoiceIssuedDomainEvent>();
    }

    [Fact]
    public void Issue_WhenAlreadyIssued_ThrowsStateConflictException()
    {
        // Arrange
        var invoice = CreateValidInvoice();
        invoice.Issue();

        // Act
        var act = () => invoice.Issue();

        // Assert
        act.Should().Throw<StateConflictException>().WithMessage("*rascunho*");
    }

    [Fact]
    public void MarkAsPaid_WhenIssued_TransitionsToPaidAndRaisesEvent()
    {
        // Arrange
        var invoice = CreateValidInvoice();
        invoice.Issue();
        invoice.ClearDomainEvents();
        var paidAt = DateTime.UtcNow;

        // Act
        invoice.MarkAsPaid(paidAt);

        // Assert
        invoice.Status.Should().Be(InvoiceStatus.Paid);
        invoice.PaidAt.Should().Be(paidAt);
        invoice.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<InvoicePaidDomainEvent>();
    }

    [Fact]
    public void MarkAsPaid_WhenDraft_ThrowsStateConflictException()
    {
        // Arrange
        var invoice = CreateValidInvoice();

        // Act
        var act = () => invoice.MarkAsPaid();

        // Assert
        act.Should().Throw<StateConflictException>().WithMessage("*rascunho*emitida*");
    }

    [Fact]
    public void MarkAsPaid_WhenAlreadyPaid_ThrowsStateConflictException()
    {
        // Arrange
        var invoice = CreateValidInvoice();
        invoice.Issue();
        invoice.MarkAsPaid();

        // Act
        var act = () => invoice.MarkAsPaid();

        // Assert
        act.Should().Throw<StateConflictException>().WithMessage("*já está paga*");
    }

    [Fact]
    public void MarkAsPaid_WhenCancelled_ThrowsStateConflictException()
    {
        // Arrange
        var invoice = CreateValidInvoice();
        invoice.Cancel();

        // Act
        var act = () => invoice.MarkAsPaid();

        // Assert
        act.Should().Throw<StateConflictException>().WithMessage("*cancelada*");
    }

    [Fact]
    public void Cancel_WhenDraft_TransitionsToCancelledAndRaisesEvent()
    {
        // Arrange
        var invoice = CreateValidInvoice();
        invoice.ClearDomainEvents();
        var cancelledAt = DateTime.UtcNow;

        // Act
        invoice.Cancel(cancelledAt);

        // Assert
        invoice.Status.Should().Be(InvoiceStatus.Cancelled);
        invoice.CancelledAt.Should().Be(cancelledAt);
        invoice.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<InvoiceCancelledDomainEvent>();
    }

    [Fact]
    public void Cancel_WhenIssued_TransitionsToCancelled()
    {
        // Arrange
        var invoice = CreateValidInvoice();
        invoice.Issue();

        // Act
        invoice.Cancel();

        // Assert
        invoice.Status.Should().Be(InvoiceStatus.Cancelled);
    }

    [Fact]
    public void Cancel_WhenPaid_ThrowsStateConflictException()
    {
        // Arrange
        var invoice = CreateValidInvoice();
        invoice.Issue();
        invoice.MarkAsPaid();

        // Act
        var act = () => invoice.Cancel();

        // Assert
        act.Should().Throw<StateConflictException>().WithMessage("*paga*não pode ser cancelada*");
    }

    [Fact]
    public void Cancel_WhenAlreadyCancelled_ThrowsStateConflictException()
    {
        // Arrange
        var invoice = CreateValidInvoice();
        invoice.Cancel();

        // Act
        var act = () => invoice.Cancel();

        // Assert
        act.Should().Throw<StateConflictException>().WithMessage("*já está cancelada*");
    }

    [Fact]
    public void ClearDomainEvents_AfterEventRaised_RemovesEvents()
    {
        // Arrange
        var invoice = CreateValidInvoice();
        invoice.Issue();
        invoice.DomainEvents.Should().NotBeEmpty();

        // Act
        invoice.ClearDomainEvents();

        // Assert
        invoice.DomainEvents.Should().BeEmpty();
    }

    private Invoice CreateValidInvoice()
    {
        return Invoice.Create(
            customerId: Guid.NewGuid(),
            number: "INV-001",
            total: _fixture.Create<Money>(),
            dueDate: DateTime.UtcNow.AddDays(30));
    }
}