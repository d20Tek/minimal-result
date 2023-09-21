//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Minimal.Result;

public class Result<TValue> : Result
{
    public new TValue Value => 
        ValueOrDefault ??
        throw new InvalidOperationException("Value should not be null in this instance.");

    public new TValue? ValueOrDefault { get; init; }

    protected Result(TValue value)
        : base(value)
    {
        ValueOrDefault = value;
    }

    protected Result(Error error)
        : base(error)
    {
    }

    protected Result(IEnumerable<Error> errors)
        : base(errors)
    {
    }

    public static implicit operator Result<TValue>(TValue value) =>
        new Result<TValue>(value);

    public static implicit operator Result<TValue>(Error error) =>
        new Result<TValue>(error);

    public static implicit operator Result<TValue>(Error[] errors) =>
        new Result<TValue>(errors);

    public static implicit operator Result<TValue>(List<Error> errors) =>
        new Result<TValue>(errors);

    public static implicit operator Result<TValue>(Exception exception) =>
        new Result<TValue>(DefaultErrors.UnhandledExpection(exception.Message));

    public static Result<TValue> Success(TValue value) => new Result<TValue>(value);

    public Result<TResult> MapResult<TResult>(Func<TValue, TResult> mapper) =>
        (IsSuccess) ? mapper(Value) : ErrorsList;

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

    public void Match(Action<TValue> success, Action<IEnumerable<Error>> failure)
    {
        if (IsSuccess)
        {
            success(Value);
        }
        else
        {
            failure(Errors);
        }
    }

    public async Task MatchAsync(
        Func<TValue, Task> success,
        Func<IEnumerable<Error>, Task> failure)
    {
        if (IsSuccess)
        {
            await success(Value).ConfigureAwait(false);
        }
        else
        {
            await failure(Errors).ConfigureAwait(false);
        }
    }

    public void MatchFirstError(Action<TValue> success, Action<Error> failure)
    {
        if (IsSuccess)
        {
            success(Value);
        }
        else
        {
            failure(Errors.First());
        }
    }

    public async Task MatchFirstErrorAsync(Func<TValue, Task> success, Func<Error, Task> failure)
    {
        if (IsSuccess)
        {
            await success(Value).ConfigureAwait(false);
        }
        else
        {
            await failure(Errors.First()).ConfigureAwait(false);
        }
    }
}
