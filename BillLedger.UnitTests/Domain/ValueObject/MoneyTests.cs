namespace BillLedger.UnitTests.Domain.ValueObject;

public sealed class MoneyTests
{
    [Fact]
    public void Create_WithValidAmount_ReturnsMoney()
    {
        // Arrange
        var expectedAmount = 100.50m;
        var currency = Currency.Brl;

        // Act
        var money = Money.Create(expectedAmount, currency);

        // Assert
        money.Amount.Should().Be(expectedAmount);
        money.Currency.Should().Be(currency);
    }

    [Fact]
    public void Create_WithNegativeAmount_ThrowsDomainException()
    {
        // Arrange
        var amount = -10m;
        var currency = Currency.Brl;

        // Act
        var act = () => Money.Create(amount, currency);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*negativo*");
    }

    [Fact]
    public void Create_WithMoreThanTwoDecimals_RoundsUsingBankersRounding()
    {
        // Arrange
        var currency = Currency.Brl;

        // Act
        var roundedDownMoney = Money.Create(0.005m, currency);
        var roundedUpMoney = Money.Create(0.015m, currency);
        var roundedToEvenMoney = Money.Create(0.025m, currency);

        // Assert
        roundedDownMoney.Amount.Should().Be(0.00m);
        roundedUpMoney.Amount.Should().Be(0.02m);
        roundedToEvenMoney.Amount.Should().Be(0.02m);
    }

    [Fact]
    public void Add_WithSameCurrency_ReturnsSum()
    {
        // Arrange
        var firstAmount = Money.Create(100m, Currency.Brl);
        var secondAmount = Money.Create(50m, Currency.Brl);

        // Act
        var sum = firstAmount.Add(secondAmount);

        // Assert
        sum.Amount.Should().Be(150m);
        sum.Currency.Should().Be(Currency.Brl);
    }

    [Fact]
    public void Add_WithDifferentCurrencies_ThrowsDomainException()
    {
        // Arrange
        var brlAmount = Money.Create(100m, Currency.Brl);
        var usdAmount = Money.Create(50m, Currency.Usd);

        // Act
        var act = () => brlAmount.Add(usdAmount);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*moedas diferentes*");
    }

    [Fact]
    public void Subtract_WithResultingNegativeAmount_ThrowsDomainException()
    {
        // Arrange
        var smallerAmount = Money.Create(50m, Currency.Brl);
        var largerAmount = Money.Create(100m, Currency.Brl);

        // Act
        var act = () => smallerAmount.Subtract(largerAmount);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*negativo*");
    }

    [Fact]
    public void Subtract_WithSameCurrency_ReturnsDifference()
    {
        // Arrange
        var minuend = Money.Create(100m, Currency.Brl);
        var subtrahend = Money.Create(30m, Currency.Brl);

        // Act
        var difference = minuend.Subtract(subtrahend);

        // Assert
        difference.Amount.Should().Be(70m);
        difference.Currency.Should().Be(Currency.Brl);
    }

    [Fact]
    public void Multiply_ByInteger_ReturnsProduct()
    {
        // Arrange
        var money = Money.Create(100m, Currency.Brl);
        var multiplier = 3;

        // Act
        var product = money.Multiply(multiplier);

        // Assert
        product.Amount.Should().Be(300m);
        product.Currency.Should().Be(Currency.Brl);
    }

    [Fact]
    public void Multiply_ByNegativeInteger_ThrowsDomainException()
    {
        // Arrange
        var money = Money.Create(100m, Currency.Brl);
        var multiplier = -1;

        // Act
        var act = () => money.Multiply(multiplier);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*multiplicador*");
    }

    [Fact]
    public void Equality_WithSameAmountAndCurrency_ReturnsTrue()
    {
        // Arrange
        var firstMoney = Money.Create(100m, Currency.Brl);
        var secondMoney = Money.Create(100m, Currency.Brl);

        // Act
        var isEqual = firstMoney == secondMoney;

        // Assert
        firstMoney.Should().Be(secondMoney);
        isEqual.Should().BeTrue();
    }

    [Fact]
    public void Equality_WithDifferentCurrency_ReturnsFalse()
    {
        // Arrange
        var brlMoney = Money.Create(100m, Currency.Brl);
        var usdMoney = Money.Create(100m, Currency.Usd);

        // Act
        var isEqual = brlMoney == usdMoney;

        // Assert
        brlMoney.Should().NotBe(usdMoney);
        isEqual.Should().BeFalse();
    }
}