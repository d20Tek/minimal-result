//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Net;

namespace D20Tek.Minimal.Result.AspNetCore;

public class ErrorTypeMapper : IErrorTypeMapper
{
    private static readonly Dictionary<int, HttpStatusCode> _map = new();

    private ErrorTypeMapper()
    {
    }

    public static IErrorTypeMapper Instance { get; } = CreateDefaultMapper();

    private static IErrorTypeMapper CreateDefaultMapper() =>
        new ErrorTypeMapper().Configure();

    public IErrorTypeMapper For(int errorType, HttpStatusCode statusCode)
    {
        _map[errorType] = statusCode;
        return this;
    }

    public IErrorTypeMapper Remove(int errorType)
    {
        _map.Remove(errorType);
        return this;
    }

    public bool Contains(int errorType) => _map.ContainsKey(errorType);

    public HttpStatusCode Convert(int errorType)
    {
        if (Contains(errorType))
        {
            return _map[errorType];
        }

        return HttpStatusCode.InternalServerError;
    }

    public IErrorTypeMapper Configure(Action<IErrorTypeConfigurator>? configure = null)
    {
        var configurator = new ErrorTypeConfigurator();
        if (configure is not null)
        {
            configure(configurator);
        }

        _map.Clear();
        foreach (var entry in configurator.Build())
        {
            For(entry.ErrorType, entry.StatusCode);
        }

        return this;
    }
}
