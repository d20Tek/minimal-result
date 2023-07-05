//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Xml;

namespace D20Tek.Patterns.Result;

public class Result : IResult
{
    private readonly List<Error> _errors = new();

    public bool IsSuccess => !IsFailure;

    public bool IsFailure { get; protected set; }

    public IReadOnlyList<Error> Errors => _errors.AsReadOnly();

    protected Result()
    {
        IsFailure = false;
    }

    protected Result(Error error)
    {
        _errors.Add(error);
        IsFailure = true;
    }

    protected Result(IEnumerable<Error> errors)
    {
        _errors.AddRange(errors);
        IsFailure = true;
    }

    public static implicit operator Result(Error error) =>
        new Result(error);

    public static implicit operator Result(Error[] errors) =>
        new Result(errors);

    public static implicit operator Result(Exception exception) =>
        new Result(DefaultErrors.UnhandledExpection(exception.Message));

    public static Result Success() => new Result();

    public void IfFailure(Action<IEnumerable<Error>> failure)
    {
        if (IsFailure)
        {
            failure(Errors);
        }
    }

    public async Task IfFailureAsync(Func<IEnumerable<Error>, Task> failure)
    {
        if (IsFailure)
        {
            await failure(Errors).ConfigureAwait(false);
        }
    }

    public void IfSuccess(Action success)
    {
        if (IsSuccess)
        {
            success();
        }
    }

    public async Task IfSuccessAsync(Func<Task> success)
    {
        if (IsSuccess)
        {
            await success().ConfigureAwait(false);
        }
    }
}
