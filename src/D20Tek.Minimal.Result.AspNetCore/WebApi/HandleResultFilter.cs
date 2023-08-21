//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace D20Tek.Minimal.Result.AspNetCore.WebApi;

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
                ActionResult<IResult> actionResult;
                if (result.IsSuccess)
                {
                    actionResult = result.ValueOrDefault is null
                        ? controller.Ok()
                        : controller.Ok(result.ValueOrDefault);
                }
                else
                {
                    actionResult = controller.Problem<IResult>(result.Errors);
                }

                context.Result = actionResult.ToIActionResult();
            }
        }
    }
}
