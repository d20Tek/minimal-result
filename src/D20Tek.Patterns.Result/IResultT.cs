//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Patterns.Result;

public interface IResult<TValue> : IResult
{
    public TValue Value { get; }

    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<IEnumerable<Error>, TResult> failure);

    public Task<TResult> MatchAsync<TResult>(
        Func<TValue, Task<TResult>> success,
        Func<IEnumerable<Error>, Task<TResult>> failure);

    public TResult MatchFirstError<TResult>(
        Func<TValue, TResult> success,
        Func<Error, TResult> failure);

    public Task<TResult> MatchFirstErrorAsync<TResult>(
        Func<TValue, Task<TResult>> success,
        Func<Error, Task<TResult>> failure);
}