//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Basic.MinimalApi.Endpoints;

public static class ResultExtensions
{
    public static IResult ToApiResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap)
    {
        return result.Match<IResult>(
            success => TypedResults.Ok(responseMap(success)),
            errors => Results.Extensions.Problem(errors));
    }

    public static IResult ToApiResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response)
    {
        return result.Match<IResult>(
            success => TypedResults.Ok(response),
            errors => Results.Extensions.Problem(errors));
    }

    public static IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        string? routeName = null,
        object? routeValues = null)
    {
        return result.Match<IResult>(
            success => TypedResults.CreatedAtRoute(responseMap(success), routeName, routeValues),
            errors => Results.Extensions.Problem(errors));
    }

    public static IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        string? routeName = null,
        object? routeValues = null)
    {
        return result.Match<IResult>(
            success => TypedResults.CreatedAtRoute(response, routeName, routeValues),
            errors => Results.Extensions.Problem(errors));
    }

    public static IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        Func<TValue, TResponse> responseMap,
        string routeUri)
    {
        return result.Match<IResult>(
            success => TypedResults.Created(routeUri, responseMap(success)),
            errors => Results.Extensions.Problem(errors));
    }

    public static IResult ToCreatedApiResult<TValue, TResponse>(
        this Result<TValue> result,
        TResponse response,
        string routeUri)
    {
        return result.Match<IResult>(
            success => TypedResults.Created(routeUri, response),
            errors => Results.Extensions.Problem(errors));
    }

    public static IResult ToApiResult(this Result result)
    {
        return (result.IsSuccess) 
            ? Results.Ok() 
            : Results.Extensions.Problem(result.Errors.AsEnumerable());
    }
}
