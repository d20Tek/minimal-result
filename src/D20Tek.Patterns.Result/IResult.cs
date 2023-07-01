//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Patterns.Result;

public interface IResult
{
    public bool IsSuccess { get; }

    public bool IsFailure { get; }

    public IReadOnlyList<Error> Errors { get; }

    public void IfFailure<TResult>(Action<IEnumerable<Error>> failure);

    public Task IfFailureAsync(Func<IEnumerable<Error>, Task> failure);
}
