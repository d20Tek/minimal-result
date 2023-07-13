//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http;

namespace D20Tek.Patterns.Result.AspNetCore.MinimalApi;

public sealed class HandleResultFilter : IEndpointFilter
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
