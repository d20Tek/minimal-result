//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Minimal.Result;

public static class TaskExtensions
{
    public static Task<Result<TResult>> ContinueMergeWith<TValue, TResult>(
        this Task<Result<TValue>> resultTask,
        Func<TValue, Task<Result<TResult>>> ifSucceedingFunc)
    {
        var task = resultTask.ContinueWith(
            t => t.Result.Merge<TResult>(x => ifSucceedingFunc(x)))
            .Unwrap();
        return task;
    }
}