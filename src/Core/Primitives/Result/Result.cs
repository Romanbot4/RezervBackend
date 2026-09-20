namespace Core.Primitives.Result;

using System;
using Failure;

public class Result<TValue>
{
    private readonly TValue? _value;
    private readonly Failure? _failure;

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public TValue Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                "The value cannot be accessed. The result is marked as failure."
            );

    public Failure Failure =>
        IsFailure
            ? _failure!
            : throw new InvalidOperationException(
                "The failure cannot be accessed. The result is marked as success."
            );

    protected internal Result(bool isSuccess, Failure? failure, TValue? value)
    {
        IsSuccess = isSuccess;
        _failure = failure;
        _value = value;
    }

    public static Result<TValue> Success(TValue value) => new(true, null, value);

    public static Result<TValue> Fail(Failure failure) => new(false, failure, default);

    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<Failure, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(_value!) : onFailure(_failure!);
    }
}
