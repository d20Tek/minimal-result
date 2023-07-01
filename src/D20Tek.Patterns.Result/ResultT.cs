//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Patterns.Result;

public class Result<TValue> : Result, IResult<TValue>
{
    private readonly TValue? _value;

    public TValue Value
    {
        get
        {
            if (IsFailure || _value is null)
            {
                throw new InvalidOperationException(
                    "The result value is only valid in a successful result.");
            }

            return _value;
        }
    }

    protected Result(TValue value)
    {
        _value = value;
        IsFailure = false;
    }

    private Result(Error error)
        : base(error)
    {
    }

    private Result(IEnumerable<Error> errors)
        : base(errors)
    {
    }

    public static implicit operator Result<TValue>(TValue value) =>
        new Result<TValue>(value);

    public static implicit operator Result<TValue>(Error error) =>
        new Result<TValue>(error);

    public static implicit operator Result<TValue>(Error[] errors) =>
        new Result<TValue>(errors);

    public static implicit operator Result<TValue>(Exception exception) =>
        new Result<TValue>(DefaultErrors.UnhandledExpection(exception.Message));

    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<IEnumerable<Error>, TResult> failure) =>
        (IsSuccess) ? success(Value) : failure(Errors);

    public async Task<TResult> MatchAsync<TResult>(
        Func<TValue, Task<TResult>> success,
        Func<IEnumerable<Error>, Task<TResult>> failure)
    {
        if (IsSuccess)
        {
            return await success(Value).ConfigureAwait(false);
        }
        else
        {
            return await failure(Errors).ConfigureAwait(false);
        }
    }

    public TResult MatchFirstError<TResult>(
        Func<TValue, TResult> success,
        Func<Error, TResult> failure) =>
        (IsSuccess) ? success(Value) : failure(Errors.First());


    public async Task<TResult> MatchFirstErrorAsync<TResult>(
        Func<TValue, Task<TResult>> success,
        Func<Error, Task<TResult>> failure)
    {
        if (IsSuccess)
        {
            return await success(Value).ConfigureAwait(false);
        }
        else
        {
            return await failure(Errors.First()).ConfigureAwait(false);
        }
    }
}
