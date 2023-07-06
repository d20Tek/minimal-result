//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Patterns.Result;

public interface IResult
{
    public bool IsSuccess { get; }

    public bool IsFailure { get; }

    public IReadOnlyList<Error> Errors { get; }

    public void IfFailure(Action<IEnumerable<Error>> failure);

    public Task IfFailureAsync(Func<IEnumerable<Error>, Task> failure);

    public void IfSuccess(Action success);

    public Task IfSuccessAsync(Func<Task> success);
}
