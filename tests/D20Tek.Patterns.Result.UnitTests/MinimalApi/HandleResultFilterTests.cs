﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result.AspNetCore.MinimalApi;
using D20Tek.Patterns.Result.UnitTests.Assertions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace D20Tek.Patterns.Result.UnitTests.MinimalApi;

[TestClass]
public class HandleResultFilterTests
{
    private readonly Mock<EndpointFilterInvocationContext> _mockContext = new();

    [TestMethod]
    public async Task OnActionExecuted_WithSuccessResult_ReturnsOK()
    {
        // arrange
        var result = Result.Success();
        var filter = new HandleResultFilter();

        // act
        var apiResult = await filter.InvokeAsync(
            _mockContext.Object,
            (context) => new ValueTask<object?>(result));

        // assert
        apiResult.Should().NotBeNull();
        var res = apiResult.As<Ok>();
        res.ShouldBeOkResult();
    }

    [TestMethod]
    public async Task OnActionExecuted_WithFailureResult_ReturnsProblemDetails()
    {
        // arrange
        Result result = DefaultErrors.Conflict;
        var filter = new HandleResultFilter();

        // act
        var apiResult = await filter.InvokeAsync(
            _mockContext.Object,
            (context) => new ValueTask<object?>(result));

        // assert
        apiResult.Should().NotBeNull();
        var res = apiResult.As<ProblemHttpResult>();
        res.ShouldBeProblemResult(
            StatusCodes.Status409Conflict,
            "Conflict",
            DefaultErrors.Conflict);
    }

    [TestMethod]
    public async Task OnActionExecuted_WithOKResult_RemainsUnchanged()
    {
        // arrange
        var filter = new HandleResultFilter();

        // act
        var apiResult = await filter.InvokeAsync(
            _mockContext.Object,
            (context) => new ValueTask<object?>(new OkResult()));

        // assert
        apiResult.Should().NotBeNull();
        var res = apiResult.As<OkResult>();
    }
}
