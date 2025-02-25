namespace Kern.Queue;

/// <summary>
/// Represents a task that accepts a payload and can be executed asynchronously.
/// </summary>
/// <typeparam name="T">The type of the payload.</typeparam>
public interface IQueueTaskWithPayload<T>
{
    /// <summary>
    /// Executes the task asynchronously with the provided payload.
    /// </summary>
    /// <param name="payload">The payload for the task.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InvokeAsync(T payload);
}