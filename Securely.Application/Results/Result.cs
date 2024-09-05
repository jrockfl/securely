namespace Securely.Application.Results;

public class Result<T> : Result
{
    public T Value { get; }

    public Result(bool isSuccess, T value, IReadOnlyCollection<string> errors, FailureReason failureReason = FailureReason.None)
    : base(isSuccess, errors, failureReason) // Add this line to correctly call the base constructor
    {
        Value = value;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, new List<string>());
    }

    public static new Result<T> Fail(string error, FailureReason failureReason)
    {
        return new Result<T>(false, default(T), new List<string> { error }, failureReason);
    }

    public static new Result<T> Fail(IReadOnlyCollection<string> errors, FailureReason failureReason)
    {
        return new Result<T>(false, default(T), errors, failureReason);
    }
}

public class Result
{
    public bool IsSuccess { get; protected set; }

    public string Error => string.Join(" ", Errors);

    public IReadOnlyCollection<string> Errors { get; protected set; }

    public bool IsFailure => !IsSuccess;

    public FailureReason FailureReason { get; protected set; }

    protected Result(bool isSuccess, IReadOnlyCollection<string> errors, FailureReason failureReason = FailureReason.None)
    {
        IsSuccess = isSuccess;
        Errors = errors;
        FailureReason = failureReason;
    }

    public static Result Success()
    {
        return new Result(true, new List<string>());
    }

    public static Result Fail(string error, FailureReason failureReason)
    {
        return new Result(false, new List<string> { error }, failureReason);
    }

    public static Result Fail(IReadOnlyCollection<string> errors, FailureReason failureReason)
    {
        return new Result(false, errors, failureReason);
    }
}
