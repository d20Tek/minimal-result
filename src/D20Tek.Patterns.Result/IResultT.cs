//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Patterns.Result;

public interface IResult<TValue> : IResult
{
    public new TValue? Value { get; }

    public Result<TResult> MapResult<TResult>(Func<TValue, TResult> mapper);

    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<IEnumerable<Error>, TResult> failure);

    public void Match(Action<TValue> success, Action<IEnumerable<Error>> failure);

    public Task<TResult> MatchAsync<TResult>(
        Func<TValue, Task<TResult>> success,
        Func<IEnumerable<Error>, Task<TResult>> failure);

    public TResult MatchFirstError<TResult>(
        Func<TValue, TResult> success,
        Func<Error, TResult> failure);

    public void MatchFirstError(Action<TValue> success, Action<Error> failure);

    public Task<TResult> MatchFirstErrorAsync<TResult>(
        Func<TValue, Task<TResult>> success,
        Func<Error, Task<TResult>> failure);
}