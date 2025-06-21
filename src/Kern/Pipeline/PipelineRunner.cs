namespace Kern.Pipeline;

/// <summary>
/// Represents a composable asynchronous pipeline that transforms an input of type <typeparamref name="TIn"/>
/// to an output of type <typeparamref name="TOut"/> using Go-style error handling via <see cref="PipelineResult{T}"/>.
/// </summary>
/// <typeparam name="TIn">The input type of the pipeline.</typeparam>
/// <typeparam name="TOut">The output type of the pipeline.</typeparam>
public class PipelineRunner<TIn, TOut>
{
    private readonly Func<TIn, Task<PipelineResult<TOut>>> _func;

    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineRunner{TIn, TOut}"/> class.
    /// </summary>
    /// <param name="func">A function that defines how to transform the input to the output asynchronously.</param>
    public PipelineRunner(Func<TIn, Task<PipelineResult<TOut>>> func)
    {
        _func = func;
    }

    /// <summary>
    /// Executes the pipeline with the specified input and returns the result.
    /// </summary>
    /// <param name="input">The input value to process through the pipeline.</param>
    /// <returns>A <see cref="Task"/> that resolves to a <see cref="PipelineResult{TOut}"/> containing either the final value or an error.</returns>
    public Task<PipelineResult<TOut>> RunAsync(TIn input)
    {
        return _func(input);
    }

    /// <summary>
    /// Adds an asynchronous step to the pipeline that returns a <see cref="PipelineResult{T}"/> with the next output type.
    /// If the previous step fails, this step is skipped and the error is propagated.
    /// </summary>
    /// <typeparam name="TNext">The output type of the next step.</typeparam>
    /// <param name="next">A function that takes the current output and produces the next step asynchronously with error handling.</param>
    /// <returns>A new <see cref="PipelineRunner{TIn, TNext}"/> with the step added.</returns>
    public PipelineRunner<TIn, TNext> AddStep<TNext>(Func<TOut, Task<PipelineResult<TNext>>> next)
    {
        return new PipelineRunner<TIn, TNext>(async input =>
        {
            var current = await _func(input);
            if (!current.IsSuccess)
                return PipelineResult<TNext>.Fail(current.Error!);

            return await next(current.Value!);
        });
    }

    /// <summary>
    /// Adds a synchronous step to the pipeline that returns a <see cref="PipelineResult{T}"/>.
    /// </summary>
    /// <typeparam name="TNext">The output type of the next step.</typeparam>
    /// <param name="next">A function that transforms the output to the next result with error handling.</param>
    /// <returns>A new <see cref="PipelineRunner{TIn, TNext}"/> with the step added.</returns>
    public PipelineRunner<TIn, TNext> AddStep<TNext>(Func<TOut, PipelineResult<TNext>> next)
    {
        return AddStep(x => Task.FromResult(next(x)));
    }

    /// <summary>
    /// Adds an asynchronous step that produces a plain value and wraps it in a successful <see cref="PipelineResult{T}"/>.
    /// </summary>
    /// <typeparam name="TNext">The output type of the next step.</typeparam>
    /// <param name="next">A function that asynchronously returns the next result value.</param>
    /// <returns>A new <see cref="PipelineRunner{TIn, TNext}"/> with the step added.</returns>
    public PipelineRunner<TIn, TNext> AddStep<TNext>(Func<TOut, Task<TNext>> next)
    {
        return AddStep<TNext>(async x => PipelineResult<TNext>.Success(await next(x)));
    }

    /// <summary>
    /// Adds a synchronous step that produces a plain value and wraps it in a successful <see cref="PipelineResult{T}"/>.
    /// </summary>
    /// <typeparam name="TNext">The output type of the next step.</typeparam>
    /// <param name="next">A function that returns the next result value synchronously.</param>
    /// <returns>A new <see cref="PipelineRunner{TIn, TNext}"/> with the step added.</returns>
    public PipelineRunner<TIn, TNext> AddStep<TNext>(Func<TOut, TNext> next)
    {
        return AddStep<TNext>(x => PipelineResult<TNext>.Success(next(x)));
    }
}
