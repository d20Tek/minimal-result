//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Minimal.Result;

public class ValidationsResult
{
    private readonly List<Error> _errors = new();

    public IReadOnlyList<Error> Errors => _errors.AsReadOnly();

    public virtual bool IsValid => _errors.Count == 0;

    public Result<TValue> ToResult<TValue>(TValue? value = null)
        where TValue : class
    {
        if (IsValid)
        {
            if (value is null)
            {
                throw new InvalidOperationException();
            }

            return value;
        }

        return _errors;
    }

    public virtual void AddValidationError(string code, string message)
    {
        var error = Error.Validation(code, message);
        _errors.Add(error);
    }

    public virtual void AddValidationError(Error error)
    {
        GuardInvalidErrorObject(error);
        _errors.Add(error);
    }

    public void AddOnFailure(Func<bool> condition, Error error)
    {
        ArgumentNullException.ThrowIfNull(condition);
        GuardInvalidErrorObject(error);

        if (condition() is false)
        {
            _errors.Add(error);
        }
    }

    public static ValidationsResult operator +(ValidationsResult left, ValidationsResult right)
    {
        left._errors.AddRange(right._errors);
        return left;
    }

    private void GuardInvalidErrorObject(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        if (error.Type != ErrorType.Validation)
        {
            throw new InvalidOperationException();
        }
    }
}
