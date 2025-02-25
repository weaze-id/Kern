namespace Kern.Queue;

/// <summary>
/// Represents a task that can be enqueued and executed asynchronously.
/// </summary>
public interface IQueueTask
{
    /// <summary>
    /// Executes the task asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InvokeAsync();
}