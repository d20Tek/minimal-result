//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http;
using Api = Microsoft.AspNetCore.Http;

namespace D20Tek.Patterns.Result.AspNetCore.MinimalApi;

public static class ApiResultExtensions
{
    public static Api.IResult Problem(this IResultExtensions results, IEnumerable<Error> errors)
    {
        if (errors.Any() && errors.All(e => e.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors);
    }

    public static Api.IResult Problem(this IResultExtensions results, Error error)
    {
        if (error.Type == ErrorType.Validation)
        {
            return ValidationProblem(new List<Error> { error });
        }

        int statusCode = MapErrorsToStatusCodes(error);
        var ext = CreateErrorsExtension(error);

        return Results.Problem(statusCode: statusCode, detail: error.Message, extensions: ext);
    }

    public static Api.IResult Problem(
        this IResultExtensions results,
        int statusCode,
        string errorCode,
        string message)
    {
        var ext = CreateErrorsExtension(Error.Custom(errorCode, message, statusCode));
        return Results.Problem(statusCode: statusCode, detail: message, extensions: ext);
    }

    private static Api.IResult Problem(IEnumerable<Error> errors)
    {
        if (!errors.Any())
        {
            return Results.Problem();
        }

        var error = errors.First();
        int statusCode = MapErrorsToStatusCodes(error);
        var ext = CreateErrorsExtension(errors);

        return Results.Problem(statusCode: statusCode, detail: error.Message, extensions: ext);

    }

    private static int MapErrorsToStatusCodes(Error error)
    {
        return error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Invalid => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static Dictionary<string, object?> CreateErrorsExtension(IEnumerable<Error> errors)
    {
        return new Dictionary<string, object?>
        {
            { "errorCodes", errors.Select(e => e.ToString()) }
        };
    }

    private static Dictionary<string, object?> CreateErrorsExtension(Error error)
    {
        return new Dictionary<string, object?>
        {
            { "errorCodes", new string[] { error.ToString() } }
        };
    }

    private static Api.IResult ValidationProblem(IEnumerable<Error> errors)
    {
        var modelState = new Dictionary<string, string[]>();
        foreach (var error in errors)
        {
            modelState.Add(error.Code, new[] { error.Message });
        }

        return Results.ValidationProblem(errors: modelState);
    }
}
