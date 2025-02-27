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
    /// A semaphore that restricts access to a single process at a time for modifying queues.
    /// </summary>
    private readonly SemaphoreSlim queuesSemaphore = new(1, 1);

    /// <summary>
    /// A semaphore that restricts access to a single process at a time for modifying processes.
    /// </summary>
    private readonly SemaphoreSlim processesSemaphore = new(1, 1);

    /// <summary>
    /// Enqueues a task of type <typeparamref name="T"/> into the specified queue group.
    /// </summary>
    /// <typeparam name="T">Type of the task, which must implement <see cref="IQueueTask"/></typeparam>
    /// <param name="groupId">The ID of the queue group (default is "default").</param>
    public async Task QueueTaskAsync<T>(string groupId = "default") where T : IQueueTask
    {
        await queuesSemaphore.WaitAsync();
        try
        {
            var queue = queues.GetOrAdd(groupId, new ConcurrentQueue<Func<Task>>());
            queue.Enqueue(CreateQueueTask<T>);
        }
        finally
        {
            queuesSemaphore.Release();
        }
    }

    /// <summary>
    /// Enqueues a task of type <typeparamref name="T"/> with a payload into the specified queue group.
    /// </summary>
    /// <typeparam name="T">Type of the task, which must implement <see cref="IQueueTaskWithPayload{TPayload}"/></typeparam>
    /// <typeparam name="TPayload">Type of the payload.</typeparam>
    /// <param name="payload">The payload for the task.</param>
    /// <param name="groupId">The ID of the queue group (default is "default").</param>
    public async Task QueueTaskWithPayloadAsync<T, TPayload>(TPayload payload, string groupId = "default") where T : IQueueTaskWithPayload<TPayload>
    {
        await queuesSemaphore.WaitAsync();
        try
        {
            var queue = queues.GetOrAdd(groupId, new ConcurrentQueue<Func<Task>>());
            queue.Enqueue(() => CreateQueueTaskWithPayload<T, TPayload>(payload));
        }
        finally
        {
            queuesSemaphore.Release();
        }
    }

    /// <summary>
    /// Starts processing tasks in all queue groups that are not already running.
    /// </summary>
    public async Task ConsumeQueueAsync()
    {
        await queuesSemaphore.WaitAsync();
        await processesSemaphore.WaitAsync();

        try
        {
            var queuesToProcess = queues.Where(e =>
                !e.Value.IsEmpty &&
                !processes.ContainsKey(e.Key));

            foreach (var queue in queuesToProcess)
            {
                processes.TryAdd(queue.Key, Task.Run(() => StartGroupProcess(queue.Key)));
            }
        }
        finally
        {
            queuesSemaphore.Release();
            processesSemaphore.Release();
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

        await queuesSemaphore.WaitAsync();
        try
        {
            queues.Remove(groupId, out var removedQueue);
            if (removedQueue != null && !removedQueue.IsEmpty)
            {
                queues.TryAdd(groupId, removedQueue);
            }
        }
        finally
        {
            queuesSemaphore.Release();
        }

        await processesSemaphore.WaitAsync();
        try
        {
            processes.Remove(groupId, out _);
        }
        finally
        {
            processesSemaphore.Release();
        }

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
            try
            {
                await queueTask.InvokeAsync();
            }
            catch
            {
                logger.LogError(message: $"Unhandled exception occured while running queue task '{type}'");
            }

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
            try
            {
                await queueTask.InvokeAsync(payload);
            }
            catch
            {
                logger.LogError(message: $"Unhandled exception occured while running queue task '{type}'");
            }

            return;
        }

        logger.LogError($"Service type '{type}' is not registered");
    }
}
