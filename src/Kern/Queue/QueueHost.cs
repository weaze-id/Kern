using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kern.Queue;

/// <summary>
/// Represents a hosted service that manages and consumes queued tasks.
/// </summary>
public class QueueHost(Queue queue, ILogger<QueueHost> logger) : IHostedService
{
    /// <summary>
    /// Starts the queue host and continuously processes queued tasks.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A completed task.</returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Queue host started");

        Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                queue.ConsumeQueue();
                await Task.Delay(1000);
            }
        }, cancellationToken);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Stops the queue host and waits for all active processes to complete.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the completion of all active processes.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Queue host is stopping, waiting for all processes to be done...");
        return Task.WhenAll(queue.processes.Values);
    }
}