namespace Kern.Pipeline;

/// <summary>
/// Provides a starting point for building a pipeline.
/// </summary>
public static class PipelineBuilder
{
    /// <summary>
    /// Creates a new pipeline with the input and output type set to <typeparamref name="T"/>.
    /// The initial step simply passes the input through as a successful result.
    /// </summary>
    /// <typeparam name="T">The input and output type of the pipeline.</typeparam>
    /// <returns>
    /// A <see cref="PipelineRunner{T, T}"/> instance that can be extended with additional steps.
    /// </returns>
    public static PipelineRunner<T, T> Create<T>() =>
        new PipelineRunner<T, T>(x => Task.FromResult(PipelineResult<T>.Success(x)));
}
