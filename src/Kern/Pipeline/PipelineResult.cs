namespace Kern.Pipeline;

/// <summary>
/// Represents the result of a pipeline operation, containing either a value or an error.
/// Inspired by Go-style error handling.
/// </summary>
/// <typeparam name="T">The type of the successful result value.</typeparam>
public class PipelineResult<T>
{
    /// <summary>
    /// Gets the value of the result if the operation succeeded.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Gets the error if the operation failed.
    /// </summary>
    public Kern.Error.ErrorBase? Error { get; }

    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess => Error == null;

    /// <summary>
    /// Initializes a new successful result.
    /// </summary>
    /// <param name="value">The successful result value.</param>
    public PipelineResult(T value) => Value = value;

    /// <summary>
    /// Initializes a failed result.
    /// </summary>
    /// <param name="error">The error describing the failure.</param>
    public PipelineResult(Kern.Error.ErrorBase error) => Error = error;

    /// <summary>
    /// Creates a new successful result.
    /// </summary>
    /// <param name="value">The result value.</param>
    /// <returns>A successful <see cref="PipelineResult{T}"/> instance.</returns>
    public static PipelineResult<T> Success(T value) => new(value);

    /// <summary>
    /// Creates a new failed result.
    /// </summary>
    /// <param name="error">The error associated with the result.</param>
    /// <returns>A failed <see cref="PipelineResult{T}"/> instance.</returns>
    public static PipelineResult<T> Fail(Kern.Error.ErrorBase error) => new(error);

    /// <summary>
    /// Deconstructs the result into its value and error components.
    /// </summary>
    /// <param name="value">The result value if successful; otherwise <c>null</c>.</param>
    /// <param name="error">The error if failed; otherwise <c>null</c>.</param>
    public void Deconstruct(out T? value, out Kern.Error.ErrorBase? error)
    {
        value = Value;
        error = Error;
    }
}
