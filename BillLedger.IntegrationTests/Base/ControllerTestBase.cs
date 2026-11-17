namespace BillLedger.IntegrationTests.Base;

[Collection(nameof(ControllerTestCollection))]
public abstract class ControllerTestBase(WebApplicationFixture fixture)
{
    protected HttpClient Client { get; } = fixture.CreateClient();

    protected static string GenerateRandomValidCpf()
    {
        var random = new Random();
        var digits = new int[11];

        for (int i = 0; i < 9; i++)
            digits[i] = random.Next(0, 10);

        int sum1 = 0;
        for (int i = 0; i < 9; i++)
            sum1 += digits[i] * (10 - i);
        int remainder1 = sum1 % 11;
        digits[9] = remainder1 < 2 ? 0 : 11 - remainder1;

        int sum2 = 0;
        for (int i = 0; i < 10; i++)
            sum2 += digits[i] * (11 - i);
        int remainder2 = sum2 % 11;
        digits[10] = remainder2 < 2 ? 0 : 11 - remainder2;

        return string.Concat(digits);
    }
}
