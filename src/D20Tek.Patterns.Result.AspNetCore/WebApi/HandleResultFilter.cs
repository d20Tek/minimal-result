//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace D20Tek.Patterns.Result.AspNetCore.WebApi;

public sealed class HandleResultFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var objRes = context.Result as ObjectResult;
        if (objRes?.Value is IResult result)
        {
            if (context.Controller is ControllerBase controller)
            {
                var r = result.IsSuccess ? controller.Ok()
                                         : controller.Problem<IResult>(result.Errors);
                context.Result = r.ToIActionResult();
            }
        }
    }
}
