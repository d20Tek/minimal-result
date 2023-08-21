//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using D20Tek.Minimal.Result.UnitTests.Assertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Minimal.Result.UnitTests.MinimalApi;

[TestClass]
public sealed class HandleTypedResultFilterTests
{
    private readonly Mock<EndpointFilterInvocationContext> _mockContext = new();

    [TestMethod]
    public async Task OnActionExecuted_WithSuccessResult_ReturnsOK()
    {
        // arrange
        var response = new TestResponse(12, "foo");
        var result = Result<TestResponse>.Success(response);
        var filter = new HandleTypedResultFilter<TestResponse>();

        // act
        var apiResult = await filter.InvokeAsync(
            _mockContext.Object,
            (context) => new ValueTask<object?>(result));

        // assert
        apiResult.Should().NotBeNull();
        var res = apiResult.As<Ok<TestResponse>>();
        res.ShouldBeOkResult(12, "foo");
    }

    [TestMethod]
    public async Task OnActionExecuted_WithFailureResult_ReturnsProblemDetails()
    {
        // arrange
        Result<TestResponse> result = DefaultErrors.NotFound;
        var filter = new HandleTypedResultFilter<TestResponse>();

        // act
        var apiResult = await filter.InvokeAsync(
            _mockContext.Object,
            (context) => new ValueTask<object?>(result));

        // assert
        apiResult.Should().NotBeNull();
        var res = apiResult.As<ProblemHttpResult>();
        res.ShouldBeProblemResult(
            StatusCodes.Status404NotFound,
            "Not Found",
            DefaultErrors.NotFound);
    }

    [TestMethod]
    public async Task OnActionExecuted_WithOKResult_RemainsUnchanged()
    {
        // arrange
        var filter = new HandleTypedResultFilter<TestResponse>();

        // act
        var apiResult = await filter.InvokeAsync(
            _mockContext.Object,
            (context) => new ValueTask<object?>(new OkResult()));

        // assert
        apiResult.Should().NotBeNull();
        var res = apiResult.As<OkResult>();
    }
}
