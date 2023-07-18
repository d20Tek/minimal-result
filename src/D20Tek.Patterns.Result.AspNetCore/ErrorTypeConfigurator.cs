//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace D20Tek.Patterns.Result.AspNetCore;

internal sealed class ErrorTypeConfigurator : IErrorTypeConfigurator
{
    private readonly Dictionary<int, HttpStatusCode> _config = new();

    public ErrorTypeConfigurator()
    {
        _config.Add(ErrorType.Unexpected, HttpStatusCode.InternalServerError);
        _config.Add(ErrorType.Conflict, HttpStatusCode.Conflict);
        _config.Add(ErrorType.Validation, HttpStatusCode.BadRequest);
        _config.Add(ErrorType.Failure, HttpStatusCode.BadRequest);
        _config.Add(ErrorType.NotFound, HttpStatusCode.NotFound);
        _config.Add(ErrorType.Unauthorized, HttpStatusCode.Unauthorized);
        _config.Add(ErrorType.Forbidden, HttpStatusCode.Forbidden);
        _config.Add(ErrorType.Invalid, HttpStatusCode.UnprocessableEntity);
    }

    public IErrorTypeConfigurator For(int errorType, HttpStatusCode statusCode)
    {
        _config[errorType] = statusCode;
        return this;
    }

    public IErrorTypeConfigurator Remove(int errorType)
    {
        _config.Remove(errorType);
        return this;
    }

    public IErrorTypeConfigurator Clear()
    {
        _config.Clear();
        return this;
    }

    internal IList<ConfigEntry> Build()
    {
        var result = _config.Select(x => new ConfigEntry(x.Key, x.Value)).ToList();
        return result;
    }
}

[ExcludeFromCodeCoverage]
internal sealed record ConfigEntry(int ErrorType, HttpStatusCode StatusCode);