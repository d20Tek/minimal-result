//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http;

namespace D20Tek.Patterns.Result.AspNetCore.MinimalApi;

public sealed class HandleTypedResultFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var endpointResult = await next(context);
        if (endpointResult is Result<T> result)
        {
            var r = result.IsSuccess ? TypedResults.Ok(result.Value)
                                     : Results.Extensions.Problem(result.Errors);
            return r;
        }

        return endpointResult;
    }
}
