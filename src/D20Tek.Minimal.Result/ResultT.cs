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

    public Result<TOther> ToErrorResult<TOther>()
        where TOther : class
    {
        if (IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return ErrorsList;
    }

    public Result<TResult> MapResult<TResult>(Func<TValue, TResult> mapper) =>
        (IsSuccess) ? mapper(Value) : ErrorsList;

    public Result<TResult> Merge<TResult>(Func<TValue, Result<TResult>> ifSucceedingFunc)
    {
        if (IsSuccess)
        {
            return ifSucceedingFunc(Value);
        }

        return ErrorsList;
    }

    public async Task<Result<TResult>> Merge<TResult>(Func<TValue, Task<Result<TResult>>> ifSucceedingFunc)
    {
        if (IsSuccess)
        {
            return await ifSucceedingFunc(Value);
        }

        return ErrorsList;
    }

    public TResult IfOrElse<TResult>(Func<TValue, TResult> ifFunc, Func<IEnumerable<Error>, TResult> elseFunc)
    {
        if (IsSuccess)
        {
            return ifFunc(Value);
        }

        return elseFunc(Errors);
    }

    public void IfOrElse(Action<TValue> ifAction, Action<IEnumerable<Error>>? elseAction = null)
    {
        if (IsSuccess)
        {
            ifAction(Value);
        }
        else
        {
            elseAction?.Invoke(Errors);
        }
    }

    public Task<Result<TValue>> IfOrElse(
        Func<TValue, Task<Result<TValue>>> ifFunc,
        Func<IEnumerable<Error>, Task<Result<TValue>>>? elseFunc = null)
    {
        if (IsSuccess)
        {
            return ifFunc(Value);
        }

        if (elseFunc is not null)
        {
            return elseFunc(Errors);
        }

        return Task.FromResult(this);
    }

    public Result<TValue> IfOrElse(
        Func<TValue, Result<TValue>> ifFunc,
        Func<IEnumerable<Error>, Result<TValue>>? elseFunc = null)
    {
        if (IsSuccess)
        {
            return ifFunc(Value);
        }

        if (elseFunc is not null)
        {
            return elseFunc(Errors);
        }

        return this;
    }
}
