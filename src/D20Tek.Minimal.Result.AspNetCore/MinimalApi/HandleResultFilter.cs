//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http;
using Api = Microsoft.AspNetCore.Http;

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
            Api.IResult r;
            if (result.IsSuccess)
            {
                r = result.ValueOrDefault is null
                    ? TypedResults.Ok()
                    : TypedResults.Ok(result.ValueOrDefault);
            }
            else
            {
                r = Results.Extensions.Problem(result.Errors);
            }

            return r;
        }

        return endpointResult;
    }
}
