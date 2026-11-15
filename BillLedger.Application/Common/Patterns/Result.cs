namespace BillLedger.Application.Common.Patterns;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("Um resultado de sucesso não pode conter um erro.");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("Um resultado de falha precisa conter um erro.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Não é possível acessar o valor de um resultado de falha.");

    public static Result<TValue> Success(TValue value)
    {
        return new Result<TValue>(value, true, Error.None);
    }

    public new static Result<TValue> Failure(Error error)
    {
        return new Result<TValue>(default, false, error);
    }
}
