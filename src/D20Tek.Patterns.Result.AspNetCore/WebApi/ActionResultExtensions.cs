//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace D20Tek.Patterns.Result.AspNetCore.WebApi;

public static class ActionResultExtensions
{
    private const string _errorsExtensionName = "errors";

    public static ActionResult<TResult> Problem<TResult>(
        this ControllerBase controller,
        IEnumerable<Error> errors)
    {
        if (errors.Any() && errors.All(e => e.Type == ErrorType.Validation))
        {
            return ValidationProblem<TResult>(controller, errors);
        }
        return ProblemInternal<TResult>(controller, errors);
    }

    public static ActionResult<TResult> Problem<TResult>(
        this ControllerBase controller,
        Error error)
    {
        if (error.Type == ErrorType.Validation)
        {
            return ValidationProblem<TResult>(controller, new List<Error> { error });
        }

        int statusCode = MapErrorsToStatusCodes(error);
        var ext = CreateErrorsExtension(error);

        return controller.Problem(
            statusCode: statusCode,
            detail: error.Message,
            errorsExtension: ext);
    }

    public static ActionResult<TResult> Problem<TResult>(
        this ControllerBase controller,
        int statusCode,
        string errorCode,
        string message)
    {
        var ext = CreateErrorsExtension(Error.Custom(errorCode, message, statusCode));
        return controller.Problem(statusCode: statusCode, detail: message, errorsExtension: ext);
    }

    private static ActionResult<TResult> ProblemInternal<TResult>(
        ControllerBase controller,
        IEnumerable<Error> errors)
    {
        if (!errors.Any())
        {
            return controller.Problem();
        }

        var error = errors.First();
        int statusCode = MapErrorsToStatusCodes(error);
        var ext = CreateErrorsExtension(errors);

        return controller.Problem(statusCode: statusCode, detail: error.Message, errorsExtension: ext);
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

    private static Dictionary<string, string> CreateErrorsExtension(IEnumerable<Error> errors)
    {
        var errorsExt = new Dictionary<string, string>();
        foreach (var error in errors)
        {
            errorsExt.Add(error.Code, error.Message);
        }

        return errorsExt;
    }

    private static Dictionary<string, string> CreateErrorsExtension(Error error)
    {
        var errorsExt = new Dictionary<string, string>
        {
            { error.Code, error.Message }
        };

        return errorsExt;
    }

    private static ObjectResult Problem(
        this ControllerBase controller,
        string? detail = null,
        string? instance = null,
        int? statusCode = StatusCodes.Status500InternalServerError,
        string? title = null,
        string? type = null,
        IDictionary<string, string>? errorsExtension = null)
    {
        ProblemDetails problemDetails;
        if (controller.ProblemDetailsFactory == null)
        {
            problemDetails = new ProblemDetails
            {
                Detail = detail,
                Instance = instance,
                Status = statusCode,
                Title = title,
                Type = type,
            };
        }
        else
        {
            problemDetails = controller.ProblemDetailsFactory.CreateProblemDetails(
                controller.HttpContext,
                statusCode: statusCode,
                title: title,
                type: type,
                detail: detail,
                instance: instance);
        }

        problemDetails.Extensions.Add(_errorsExtensionName, errorsExtension);

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    private static ActionResult<TValue> ValidationProblem<TValue>(
        ControllerBase controller,
        IEnumerable<Error> errors)
    {
        var modelState = new ModelStateDictionary();
        foreach (var error in errors)
        {
            modelState.AddModelError(error.Code, error.Message);
        }

        return controller.ValidationProblem(modelState);
    }
}
