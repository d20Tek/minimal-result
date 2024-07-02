//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Minimal.Result;

public static class ResultAsyncExtensions
{
    public static async Task<Result<TResult>> Merge<TValue, TResult>(
        this Result<TValue> result,
        Func<TValue, Task<Result<TResult>>> ifSucceedingFunc)
    {
        if (result.IsSuccess)
        {
            return await ifSucceedingFunc(result.Value);
        }

        return result.ErrorsList;
    }

    public static Task<Result<TResult>> ContinueMergeWith<TValue, TResult>(
        this Task<Result<TValue>> resultTask,
        Func<TValue, Task<Result<TResult>>> ifSucceedingFunc)
    {
        var task = resultTask.ContinueWith(
            t => t.Result.Merge<TValue, TResult>(x => ifSucceedingFunc(x)))
            .Unwrap();
        return task;
    }

    public static Task<Result<TValue>> IfOrElseResult<TValue>(
        this Result<TValue> result,
        Func<TValue, Task<Result<TValue>>> ifFunc,
        Func<IEnumerable<Error>, Task<Result<TValue>>>? elseFunc = null)
    {
        if (result.IsSuccess)
        {
            return ifFunc(result.Value);
        }

        if (elseFunc is not null)
        {
            return elseFunc(result.Errors);
        }

        return Task.FromResult(result);
    }
}
