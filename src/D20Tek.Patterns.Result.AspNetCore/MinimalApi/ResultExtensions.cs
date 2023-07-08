//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http;
using Api = Microsoft.AspNetCore.Http;

namespace D20Tek.Patterns.Result.AspNetCore.MinimalApi;

public static class ResultExtensions
{
    public static Api.IResult ToApiResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap)
    {
        return result.Match<Api.IResult>(
            success => TypedResults.Ok(responseMap(success)),
            errors => Results.Extensions.Problem(errors));
    }

    public static Api.IResult ToApiResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response)
    {
        return result.Match<Api.IResult>(
            success => TypedResults.Ok(response),
            errors => Results.Extensions.Problem(errors));
    }

    public static Api.IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        string? routeName = null,
        object? routeValues = null)
    {
        return result.Match<Api.IResult>(
            success => TypedResults.CreatedAtRoute(responseMap(success), routeName, routeValues),
            errors => Results.Extensions.Problem(errors));
    }

    public static Api.IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        string? routeName = null,
        object? routeValues = null)
    {
        return result.Match<Api.IResult>(
            success => TypedResults.CreatedAtRoute(response, routeName, routeValues),
            errors => Results.Extensions.Problem(errors));
    }

    public static Api.IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        string routeUri)
    {
        return result.Match<Api.IResult>(
            success => TypedResults.Created(routeUri, responseMap(success)),
            errors => Results.Extensions.Problem(errors));
    }

    public static Api.IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        string routeUri)
    {
        return result.Match<Api.IResult>(
            success => TypedResults.Created(routeUri, response),
            errors => Results.Extensions.Problem(errors));
    }

    public static Api.IResult ToApiResult(this Result result)
    {
        return result.IsSuccess
            ? Results.Ok()
            : Results.Extensions.Problem(result.Errors.AsEnumerable());
    }
}
