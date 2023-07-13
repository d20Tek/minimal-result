//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Patterns.Result;

public class Result : IResult
{
    private const string _defaultSuccess = "Ok";
    private const string _defaultFailed = "Failed";

    private object? _value = null;
    private readonly List<Error> _errors = new();

    public IReadOnlyList<Error> Errors => _errors.AsReadOnly();

    public object Value
    {
        get => _value ?? (IsSuccess ? _defaultSuccess : _defaultFailed);
    }

    public object? ValueOrDefault
    {
        get => _value;
    }

    public bool IsSuccess => !IsFailure;

    public bool IsFailure { get; }

    protected Result()
    {
        IsFailure = false;
    }

    protected Result(object? value)
    {
        _value = value;
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

    public static implicit operator Result(List<Error> errors) =>
        new Result(errors);

    public static implicit operator Result(Exception exception) =>
        new Result(DefaultErrors.UnhandledExpection(exception.Message));

    public static Result Success() => new Result();
}
