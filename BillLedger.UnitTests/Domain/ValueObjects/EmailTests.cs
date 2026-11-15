namespace BillLedger.UnitTests.Domain.ValueObjects;

public sealed class EmailTests
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("first.last@example.com")]
    [InlineData("user+tag@example.com")]
    [InlineData("user@sub.example.com")]
    public void Create_WithValidEmail_ReturnsEmail(string validEmail)
    {
        // Arrange & Act
        var email = Email.Create(validEmail);

        // Assert
        email.Value.Should().Be(validEmail);
    }

    [Theory]
    [InlineData("USER@EXAMPLE.COM")]
    [InlineData("User@Example.Com")]
    public void Create_WithUppercaseEmail_NormalizesToLowercase(string input)
    {
        // Arrange
        var expected = input.ToLowerInvariant();

        // Act
        var email = Email.Create(input);

        // Assert
        email.Value.Should().Be(expected);
    }

    [Fact]
    public void Create_WithWhitespaceAroundEmail_TrimsValue()
    {
        // Arrange
        var paddedEmail = "  user@example.com  ";

        // Act
        var email = Email.Create(paddedEmail);

        // Assert
        email.Value.Should().Be("user@example.com");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithNullOrEmptyEmail_ThrowsDomainException(string? invalidEmail)
    {
        // Arrange & Act
        var act = () => Email.Create(invalidEmail!);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*vazio*");
    }

    [Theory]
    [InlineData("userexample.com")]
    [InlineData("user@")]
    [InlineData("@example.com")]
    [InlineData("user@example")]
    [InlineData("user @example.com")]
    [InlineData("user@exa mple.com")]
    [InlineData("a@b@c.com")]
    public void Create_WithInvalidFormat_ThrowsDomainException(string invalidEmail)
    {
        // Arrange & Act
        var act = () => Email.Create(invalidEmail);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*inválido*");
    }

    [Fact]
    public void Equality_WithSameNormalizedValue_ReturnsTrue()
    {
        // Arrange
        var firstEmail = Email.Create("user@example.com");
        var secondEmail = Email.Create("USER@EXAMPLE.COM");

        // Act
        var isEqual = firstEmail == secondEmail;

        // Assert
        isEqual.Should().BeTrue();
    }

    [Fact]
    public void Equality_WithDifferentValue_ReturnsFalse()
    {
        // Arrange
        var firstEmail = Email.Create("user@example.com");
        var secondEmail = Email.Create("other@example.com");

        // Act
        var isEqual = firstEmail == secondEmail;

        // Assert
        isEqual.Should().BeFalse();
    }
}