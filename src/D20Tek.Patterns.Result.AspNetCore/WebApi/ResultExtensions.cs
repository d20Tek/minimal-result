//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Patterns.Result.AspNetCore.WebApi;

public static class ResultExtensions
{
    public static ActionResult<TResponse> ToActionResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller)
    {
        return result.Match<ActionResult<TResponse>>(
            success => controller.Ok(responseMap(success)),
            errors => controller.Problem<TResponse>(errors));
    }

    public static ActionResult<TResponse> ToCreatedAtActionResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller,
        string? routeName = null,
        object? routeValues = null)
    {
        return result.Match<ActionResult<TResponse>>(
            success => controller.CreatedAtAction(routeName, routeValues, responseMap(success)),
            errors => controller.Problem<TResponse>(errors));
    }
}
