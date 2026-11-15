namespace BillLedger.Application.Common.Patterns;

public sealed record Error(int Code, string Message)
{
    public static readonly Error None = new(0, string.Empty);
}
