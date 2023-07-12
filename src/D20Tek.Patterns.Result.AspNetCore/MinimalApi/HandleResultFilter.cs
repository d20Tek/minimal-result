//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result.AspNetCore.MinimalApi;
using Microsoft.AspNetCore.Http;

namespace D20Tek.Patterns.Result.AspNetCore.WebApi;

public sealed class HandleResultEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var endpointResult = await next(context);
        if (endpointResult is IResult result)
        {
            var r = result.IsSuccess ? TypedResults.Ok()
                                     : Results.Extensions.Problem(result.Errors);
            return r;
        }

        return endpointResult;
    }
}
