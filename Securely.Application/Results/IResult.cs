namespace Securely.Application.Results;
/// <summary>
/// Represents the result of an operation, which can be either successful or failed.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public interface IResult<T>
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Indicates whether the operation was a failure.
    /// </summary>
    bool IsFailure { get; }

    /// <summary>
    /// The result value if the operation was successful.
    /// </summary>
    T Value { get; }

    /// <summary>
    /// An error message if the operation failed.
    /// </summary>
    string Error { get; }

    /// <summary>
    /// The reason for the failure.
    /// </summary>
    FailureReason FailureReason { get; }

    /// <summary>
    /// The list of error messages if the operation failed.
    /// </summary>
    IReadOnlyCollection<string> Errors { get; }
}
