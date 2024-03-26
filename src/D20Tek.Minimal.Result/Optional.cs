//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Minimal.Result;

public class Optional<TValue> : IEquatable<Optional<TValue>>
    where TValue : class
{
    private readonly TValue? _value;

    public TValue Value =>
        _value ?? throw new InvalidOperationException("Value should not be null in this instance.");

    public bool HasValue => _value != null;

    private Optional(TValue? value)
    {
        _value = value;
    }

    public static implicit operator Optional<TValue>(TValue? value) => new(value);
    
    public static Optional<TValue> Of(TValue value) => new(value);

    public static Optional<TValue> OfNullable(TValue? value) => new(value);

    public static Optional<TValue> Empty() => new(null);

    public TValue OrElse(TValue other) => HasValue ? Value : other;

    public TValue OrElse(Func<TValue> supplier) => OrElse(supplier());

    public void IfPresentOrElse(Action<TValue> value, Action elseAction)
    {
        if (HasValue)
        {
            value(Value);
        }
        else
        {
            elseAction();
        }
    }

    public TResult IfPresentOrElse<TResult>(Func<TValue, TResult> value, Func<TResult> elseFunc)
    {
        if (HasValue)
        {
            return value(Value);
        }
        else
        {
            return elseFunc();
        }
    }

    public override string ToString() => _value?.ToString() ?? string.Empty;

    public override int GetHashCode() => _value?.GetHashCode() ?? 0;

    public override bool Equals(object? obj)
    {
        if (obj is Optional<TValue> other)
        {
            return Equals(other);
        }

        return false;
    }

    public bool Equals(Optional<TValue>? other)
    {
        if (other is null) return false;

        return _value?.Equals(other._value) ?? false;
    }
}
