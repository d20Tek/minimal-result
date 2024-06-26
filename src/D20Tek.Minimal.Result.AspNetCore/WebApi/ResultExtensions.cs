﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Minimal.Result.AspNetCore.WebApi;

public static class ResultExtensions
{
    public static ActionResult<TResponse> ToActionResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller)
    {
        return result.IfOrElse<ActionResult<TResponse>>(
            success => controller.Ok(responseMap(success)),
            errors => controller.Problem<TResponse>(errors));
    }

    public static ActionResult<TResponse> ToActionResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        ControllerBase controller)
    {
        return result.IfOrElse<ActionResult<TResponse>>(
            success => controller.Ok(response),
            errors => controller.Problem<TResponse>(errors));
    }

    public static ActionResult<TResponse> ToActionResult<TResponse>(
        this Result result,
        ControllerBase controller)
    {
        return result.IsSuccess
            ? controller.Ok()
            : controller.Problem<TResponse>(result.Errors);
    }

    public static ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller,
        string? routeName = null,
        object? routeValues = null)
    {
        return result.IfOrElse<ActionResult<TResponse>>(
            success => controller.CreatedAtAction(routeName, routeValues, responseMap(success)),
            errors => controller.Problem<TResponse>(errors));
    }

    public static ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        ControllerBase controller,
        string? routeName = null,
        object? routeValues = null)
    {
        return result.IfOrElse<ActionResult<TResponse>>(
            success => controller.CreatedAtAction(routeName, routeValues, response),
            errors => controller.Problem<TResponse>(errors));
    }

    public static ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        ControllerBase controller,
        string routeUri)
    {
        return result.IfOrElse<ActionResult<TResponse>>(
            success => controller.Created(routeUri, responseMap(success)),
            errors => controller.Problem<TResponse>(errors));
    }

    public static ActionResult<TResponse> ToCreatedActionResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        ControllerBase controller,
        string routeUri)
    {
        return result.IfOrElse<ActionResult<TResponse>>(
            success => controller.Created(routeUri, response),
            errors => controller.Problem<TResponse>(errors));
    }
}
