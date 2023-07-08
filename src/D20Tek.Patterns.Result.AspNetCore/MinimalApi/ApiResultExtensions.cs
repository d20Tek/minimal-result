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
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors);
    }

    public static Api.IResult Problem(this IResultExtensions results, Error error)
    {
        var errors = new List<Error> { error };
        if (error.Type == ErrorType.Validation)
        {
            return ValidationProblem(errors);
        }

        return Problem(errors);
    }

    public static Api.IResult Problem(
        this IResultExtensions results,
        int statusCode,
        string errorCode,
        string message)
    {
        var ext = new Dictionary<string, object?>
        {
            { "errorCodes", new object[] { errorCode } }
        };

        return Results.Problem(
                statusCode: statusCode,
                detail: message,
                extensions: ext);
    }

    private static Api.IResult Problem(IEnumerable<Error> errors)
    {
        if (!errors.Any())
        {
            return Results.Problem();
        }

        var firstError = errors.First();

        var statusCode = firstError.Type switch
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

        var ext = new Dictionary<string, object?>
        {
            { "errorCodes", errors.Select(e => e.Code) }
        };

        return Results.Problem(
            statusCode: statusCode,
            detail: firstError.Message,
            extensions: ext);
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
