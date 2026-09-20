namespace Core.Primitives.Result;

using Failure;

public static class ResultExtension
{
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapper)
    {
        return result.IsSuccess
            ? Result<TOut>.Success(mapper(result.Value))
            : Result<TOut>.Fail(result.Failure);
    }

    public static async Task<TOut> When<TIn, TOut>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, TOut> onSuccess,
        Func<Failure, TOut> onFailure
    )
    {
        var result = await resultTask;

        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Failure);
    }

    public static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> func
    )
    {
        if (!result.IsSuccess)
        {
            return Result<TOut>.Fail(result.Failure);
        }

        return await func(result.Value);
    }
}
