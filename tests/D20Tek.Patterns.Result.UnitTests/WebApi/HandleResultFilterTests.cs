//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result.AspNetCore.WebApi;
using D20Tek.Patterns.Result.UnitTests.Assertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace D20Tek.Patterns.Result.UnitTests.WebApi;

[TestClass]
public sealed class HandleResultFilterTests
{
    private readonly TestController _controller = new();
    private readonly List<IFilterMetadata> _filters = new();
    private readonly Dictionary<string, object?> _arguments = new();


    [TestMethod]
    public void OnActionExecuting_DoesNothing()
    {
        // arrange
        var result = new OkResult();
        var executingContext = CreateActionExecutingContext(result);
        var filter = new HandleResultFilter();

        // act
        filter.OnActionExecuting(executingContext);

        // assert
        executingContext.Result.Should().Be(result);
    }

    [TestMethod]
    public void OnActionExecuted_WithSuccessResult_ReturnsOK()
    {
        // arrange
        var executedContext = CreateActionExecutedContext(Result.Success());
        var filter = new HandleResultFilter();

        // act
        filter.OnActionExecuted(executedContext);

        // assert
        executedContext.Result.Should().NotBeNull();
        executedContext.Result.Should().BeOfType<OkResult>();
    }

    [TestMethod]
    public void OnActionExecuted_WithFailureResult_ReturnsProblemDetails()
    {
        // arrange
        var executedContext = CreateActionExecutedContext(DefaultErrors.NotFound);

        var filter = new HandleResultFilter();

        // act
        filter.OnActionExecuted(executedContext);

        // assert
        executedContext.Result.Should().NotBeNull();
        executedContext.Result!.ShouldBeProblemResult(
            StatusCodes.Status404NotFound,
            DefaultErrors.NotFound);
    }

    [TestMethod]
    public void OnActionExecuted_WithOKResult_RemainsUnchanged()
    {
        // arrange
        var executedContext = CreateActionExecutedContext(new OkResult());
        var filter = new HandleResultFilter();

        // act
        filter.OnActionExecuted(executedContext);

        // assert
        executedContext.Result.Should().NotBeNull();
        executedContext.Result.Should().BeOfType<OkResult>();
    }

    [TestMethod]
    public void OnActionExecuted_WithUnexpectedControllerType_RemainsUnchanged()
    {
        // arrange
        var executedContext = CreateActionExecutedContext(
            DefaultErrors.NotFound,
            "controller");

        var filter = new HandleResultFilter();

        // act
        filter.OnActionExecuted(executedContext);

        // assert
        executedContext.Result.Should().NotBeNull();
        executedContext.Result.Should().BeOfType<ObjectResult>();
    }

    private ActionExecutedContext CreateActionExecutedContext(
        Result result,
        object? controller = null)
    {
        var executedContext = new ActionExecutedContext(
            CreateActionContext(),
            _filters,
            controller ?? _controller)
        {
            Result = new ObjectResult(result)
        };

        return executedContext;
    }

    private ActionExecutedContext CreateActionExecutedContext(IActionResult result)
    {
        var executedContext = new ActionExecutedContext(
            CreateActionContext(),
            _filters,
            _controller)
        {
            Result = result
        };

        return executedContext;
    }

    private ActionExecutingContext CreateActionExecutingContext(IActionResult result)
    {
        var executingContext = new ActionExecutingContext(
            CreateActionContext(),
            _filters,
            _arguments,
            _controller)
        {
            Result = result
        };

        return executingContext;
    }

    private static ActionContext CreateActionContext()
    {
        var httpContext = new Mock<HttpContext>();

        return new ActionContext(
            httpContext.Object,
            new RouteData(),
            new ActionDescriptor());
    }
}
