namespace BillLedger.UnitTests.Domain.ValueObjects;

public sealed class CpfTests
{
    private const string ValidRawCpf = "12345678909";
    private const string ValidFormattedCpf = "123.456.789-09";

    [Theory]
    [InlineData("12345678909")]
    [InlineData("52998224725")]
    [InlineData("11144477735")]
    public void Create_WithValidRawCpf_ReturnsCpf(string rawCpf)
    {
        // Arrange & Act
        var cpf = Cpf.Create(rawCpf);

        // Assert
        cpf.Value.Should().Be(rawCpf);
    }

    [Theory]
    [InlineData("123.456.789-09", "12345678909")]
    [InlineData("529.982.247-25", "52998224725")]
    [InlineData("111.444.777-35", "11144477735")]
    public void Create_WithValidFormattedCpf_NormalizesToDigitsOnly(string formattedCpf, string expectedDigits)
    {
        // Arrange & Act
        var cpf = Cpf.Create(formattedCpf);

        // Assert
        cpf.Value.Should().Be(expectedDigits);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrEmptyCpf_ThrowsDomainException(string? invalidCpf)
    {
        // Arrange & Act
        var act = () => Cpf.Create(invalidCpf!);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*vazio*");
    }

    [Theory]
    [InlineData("123")]
    [InlineData("123456789")]
    [InlineData("123456789012")]
    public void Create_WithWrongLength_ThrowsDomainException(string invalidCpf)
    {
        // Arrange & Act
        var act = () => Cpf.Create(invalidCpf);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*11 dígitos*");
    }

    [Theory]
    [InlineData("00000000000")]
    [InlineData("11111111111")]
    [InlineData("22222222222")]
    [InlineData("99999999999")]
    public void Create_WithAllSameDigits_ThrowsDomainException(string invalidCpf)
    {
        // Arrange & Act
        var act = () => Cpf.Create(invalidCpf);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*dígitos iguais*");
    }

    [Theory]
    [InlineData("12345678900")]
    [InlineData("52998224724")]
    [InlineData("12345678910")]
    public void Create_WithInvalidCheckDigits_ThrowsDomainException(string invalidCpf)
    {
        // Arrange & Act
        var act = () => Cpf.Create(invalidCpf);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*verificadores*");
    }

    [Fact]
    public void ToString_ReturnsFormattedCpf()
    {
        // Arrange
        var cpf = Cpf.Create(ValidRawCpf);

        // Act
        var formatted = cpf.ToString();

        // Assert
        formatted.Should().Be(ValidFormattedCpf);
    }

    [Fact]
    public void Equality_WithSameDigitsInDifferentFormats_ReturnsTrue()
    {
        // Arrange
        var firstCpf = Cpf.Create("12345678909");
        var secondCpf = Cpf.Create("123.456.789-09");

        // Act
        var isEqual = firstCpf == secondCpf;

        // Assert
        isEqual.Should().BeTrue();
    }

    [Fact]
    public void Equality_WithDifferentDigits_ReturnsFalse()
    {
        // Arrange
        var firstCpf = Cpf.Create("12345678909");
        var secondCpf = Cpf.Create("52998224725");

        // Act
        var isEqual = firstCpf == secondCpf;

        // Assert
        isEqual.Should().BeFalse();
    }
}
