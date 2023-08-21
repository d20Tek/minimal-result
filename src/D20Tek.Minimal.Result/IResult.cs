//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Minimal.Result;

public interface IResult
{
    public object Value { get; }

    public object? ValueOrDefault { get; }

    public IReadOnlyList<Error> Errors { get; }

    public bool IsSuccess { get; }

    public bool IsFailure { get; }
}
