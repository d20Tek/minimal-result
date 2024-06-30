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

    public TResult IfOrElse<TResult>(
        Func<TValue, TResult> success,
        Func<IEnumerable<Error>, TResult> failure)
    {
        if (IsSuccess)
        {
            return success(Value);
        }

        return failure(Errors);
    }

    public void IfOrElse(Action<TValue> success, Action<IEnumerable<Error>> failure)
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
}
