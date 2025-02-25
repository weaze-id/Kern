using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Kern.Queue;

/// <summary>
/// Represents a grouped queue processor that manages task execution in named queues.
/// </summary>
public class Queue(IServiceScopeFactory serviceScopeFactory, ILogger<Queue> logger)
{
    /// <summary>
    /// Dictionary storing queues for different group IDs.
    /// </summary>
    public readonly ConcurrentDictionary<string, ConcurrentQueue<Func<Task>>> queues = new();

    /// <summary>
    /// Dictionary tracking active processes for each queue group.
    /// </summary>
    public readonly ConcurrentDictionary<string, Task> processes = new();

    /// <summary>
    /// Enqueues a task of type <typeparamref name="T"/> into the specified queue group.
    /// </summary>
    /// <typeparam name="T">Type of the task, which must implement <see cref="IQueueTask"/></typeparam>
    /// <param name="groupId">The ID of the queue group (default is "default").</param>
    public void QueueTask<T>(string groupId = "default") where T : IQueueTask
    {
        var queue = queues.GetOrAdd(groupId, new ConcurrentQueue<Func<Task>>());
        queue.Enqueue(CreateQueueTask<T>);
    }

    /// <summary>
    /// Enqueues a task of type <typeparamref name="T"/> with a payload into the specified queue group.
    /// </summary>
    /// <typeparam name="T">Type of the task, which must implement <see cref="IQueueTaskWithPayload{TPayload}"/></typeparam>
    /// <typeparam name="TPayload">Type of the payload.</typeparam>
    /// <param name="payload">The payload for the task.</param>
    /// <param name="groupId">The ID of the queue group (default is "default").</param>
    public void QueueTaskWithPayload<T, TPayload>(TPayload payload, string groupId = "default") where T : IQueueTaskWithPayload<TPayload>
    {
        var queue = queues.GetOrAdd(groupId, new ConcurrentQueue<Func<Task>>());
        queue.Enqueue(() => CreateQueueTaskWithPayload<T, TPayload>(payload));
    }

    /// <summary>
    /// Starts processing tasks in all queue groups that are not already running.
    /// </summary>
    public void ConsumeQueue()
    {
        foreach (var queue in queues)
        {
            if (!processes.ContainsKey(queue.Key) && !queue.Value.IsEmpty)
            {
                processes.GetOrAdd(queue.Key, Task.Run(() => StartGroupProcess(queue.Key)));
            }
        }
    }

    /// <summary>
    /// Processes tasks in the queue for the specified group ID.
    /// </summary>
    /// <param name="groupId">The ID of the queue group.</param>
    private async Task StartGroupProcess(string groupId)
    {
        logger.LogInformation($"Queue process for '{groupId}' started");

        while (queues.TryGetValue(groupId, out var queue) && !queue.IsEmpty)
        {
            if (queue.TryDequeue(out var queueTask))
            {
                await queueTask();
            }
        }

        processes.Remove(groupId, out _);
        logger.LogInformation($"Queue process for '{groupId}' is done");
    }

    /// <summary>
    /// Creates and invokes a task of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type of the task, which must implement <see cref="IQueueTask"/></typeparam>
    private async Task CreateQueueTask<T>() where T : IQueueTask
    {
        var type = typeof(T);
        using var scope = serviceScopeFactory.CreateScope();
        if (scope.ServiceProvider.GetService<T>() is IQueueTask queueTask)
        {
            await queueTask.InvokeAsync();
            return;
        }

        logger.LogError($"Service type '{type}' is not registered");
    }

    /// <summary>
    /// Creates and invokes a task of type <typeparamref name="T"/> with a payload.
    /// </summary>
    /// <typeparam name="T">Type of the task, which must implement <see cref="IQueueTaskWithPayload{TPayload}"/></typeparam>
    /// <typeparam name="TPayload">Type of the payload.</typeparam>
    /// <param name="payload">The payload for the task.</param>
    private async Task CreateQueueTaskWithPayload<T, TPayload>(TPayload payload) where T : IQueueTaskWithPayload<TPayload>
    {
        var type = typeof(T);
        using var scope = serviceScopeFactory.CreateScope();
        if (scope.ServiceProvider.GetService<T>() is IQueueTaskWithPayload<TPayload> queueTask)
        {
            await queueTask.InvokeAsync(payload);
            return;
        }

        logger.LogError($"Service type '{type}' is not registered");
    }
}
