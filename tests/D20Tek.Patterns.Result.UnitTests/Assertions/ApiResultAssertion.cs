//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result.UnitTests.MinimalApi;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace D20Tek.Patterns.Result.UnitTests.Assertions;

internal static class ApiResultAssertion
{
    public static void ShouldBeOkResult(
        this Microsoft.AspNetCore.Http.IResult apiResult,
        int expectedId,
        string expecteMessage)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<Ok<TestResponse>>();

        var response = apiResult.As<Ok<TestResponse>>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        response.Value.Should().NotBeNull();
        response.Value.Should().BeOfType<TestResponse>();
        response.Value!.Id.Should().Be(expectedId);
        response.Value!.Message.Should().Be(expecteMessage);
    }

    public static void ShouldBeCreatedAtResult(
        this Microsoft.AspNetCore.Http.IResult apiResult,
        int expectedId,
        string expecteMessage)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<CreatedAtRoute<TestResponse>>();

        var response = apiResult.As<CreatedAtRoute<TestResponse>>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(StatusCodes.Status201Created);
        response.Value.Should().NotBeNull();
        response.Value.Should().BeOfType<TestResponse>();
        response.Value!.Id.Should().Be(expectedId);
        response.Value!.Message.Should().Be(expecteMessage);
    }

    public static void ShouldBeCreatedResult(
        this Microsoft.AspNetCore.Http.IResult apiResult,
        string expectedUri,
        int expectedId,
        string expecteMessage)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<Created<TestResponse>>();

        var response = apiResult.As<Created<TestResponse>>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(StatusCodes.Status201Created);
        response.Location.Should().Be(expectedUri);
        response.Value.Should().NotBeNull();
        response.Value.Should().BeOfType<TestResponse>();
        response.Value!.Id.Should().Be(expectedId);
        response.Value!.Message.Should().Be(expecteMessage);
    }

    public static void ShouldBeOkResult(
        this Microsoft.AspNetCore.Http.IResult apiResult)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<Ok>();

        var response = apiResult.As<Ok>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    public static void ShouldBeProblemResult(
        this Microsoft.AspNetCore.Http.IResult apiResult,
        int expectedStatusCode,
        string expectedTitle,
        Error expectedError)
    {
        apiResult.Should().NotBeNull();
        apiResult.Should().BeOfType<ProblemHttpResult>();

        var response = apiResult.As<ProblemHttpResult>();
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(expectedStatusCode);
        response.ProblemDetails.Status.Should().Be(expectedStatusCode);
        response.ProblemDetails.Detail.Should().Be(expectedError.Message);
        response.ProblemDetails.Title.Should().Be(expectedTitle);
        response.ProblemDetails.Extensions.Should().HaveCount(1);

        var ext = response.ProblemDetails.Extensions.First();
        ext.Key.Should().Be("errorCodes");
        var errorCodes = ext.Value as IEnumerable<string>;
        errorCodes.Should().NotBeNull();
        errorCodes.Should().HaveCount(1);
        errorCodes!.First().ToString().Should().Be(expectedError.Code);
    }
}
