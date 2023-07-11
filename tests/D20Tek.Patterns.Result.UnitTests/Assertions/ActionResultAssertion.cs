//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Patterns.Result.UnitTests.Assertions;

internal static class ActionResultAssertion
{
    public static void ShouldBeOkResult(
        this ActionResult<TestResponse> actionResult,
        int expectedId,
        string expecteMessage)
    {
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType<ActionResult<TestResponse>>();
        actionResult.Result.Should().BeOfType<OkObjectResult>();

        var okResult = actionResult.Result.As<OkObjectResult>();
        okResult.Should().NotBeNull();

        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Should().BeOfType<TestResponse>();

        var value = okResult.Value.As<TestResponse>();
        value.Should().NotBeNull();
        value!.Id.Should().Be(expectedId);
        value!.Message.Should().Be(expecteMessage);
    }

    public static void ShouldBeCreatedAtResult(
        this ActionResult<TestResponse> actionResult,
        int expectedId,
        string expecteMessage)
    {
        actionResult.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<CreatedAtActionResult>();

        var createdResult = actionResult.Result.As<CreatedAtActionResult>();
        createdResult.Should().NotBeNull();

        createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdResult.Value.Should().NotBeNull();
        createdResult.Value.Should().BeOfType<TestResponse>();

        var response = createdResult.Value.As<TestResponse>();
        response.Should().NotBeNull();
        response.Should().BeOfType<TestResponse>();
        response.Id.Should().Be(expectedId);
        response.Message.Should().Be(expecteMessage);
    }

    public static void ShouldBeCreatedResult(
        this ActionResult<TestResponse> actionResult,
        string expectedUri,
        int expectedId,
        string expecteMessage)
    {
        actionResult.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<CreatedResult>();

        var createdResult = actionResult.Result.As<CreatedResult>();
        createdResult.Should().NotBeNull();

        createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        createdResult.Location.Should().Be(expectedUri);
        createdResult.Value.Should().NotBeNull();
        createdResult.Value.Should().BeOfType<TestResponse>();

        var response = createdResult.Value.As<TestResponse>();
        response.Should().NotBeNull();
        response.Should().BeOfType<TestResponse>();
        response.Id.Should().Be(expectedId);
        response.Message.Should().Be(expecteMessage);
    }

    public static void ShouldBeProblemResult(
        this ActionResult<TestResponse> actionResult,
        int expectedStatusCode,
        Error? expectedError = null)
    {
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType<ActionResult<TestResponse>>();
        actionResult.Result.Should().BeOfType<ObjectResult>();

        var objResult = actionResult.Result.As<ObjectResult>();
        objResult.Should().NotBeNull();
        objResult.StatusCode.Should().Be(expectedStatusCode);
        objResult.Value.Should().BeOfType<ProblemDetails>();

        var response = objResult.Value.As<ProblemDetails>();
        response.Should().NotBeNull();
        response.Status.Should().Be(expectedStatusCode);
        response.Detail.Should().Be(expectedError?.Message);

        if (expectedError != null)
        {
            response.Extensions.Should().HaveCount(1);

            var ext = response.Extensions.First();
            ext.Key.Should().Be("errors");
            var errors = ext.Value as IDictionary<string, string>;
            errors.Should().NotBeNull();
            errors.Should().HaveCount(1);
            errors.Should().ContainKey(expectedError.Value.Code);
            errors.Should().ContainValue(expectedError.Value.Message);
        }
    }

    public static void ShouldBeProblemResult(
        this ActionResult<TestResponse> actionResult,
        int expectedStatusCode,
        IEnumerable<Error> expectedErrors)
    {
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType<ActionResult<TestResponse>>();
        actionResult.Result.Should().BeOfType<ObjectResult>();

        var objResult = actionResult.Result.As<ObjectResult>();
        objResult.Should().NotBeNull();
        objResult.StatusCode.Should().Be(expectedStatusCode);
        objResult.Value.Should().BeOfType<ProblemDetails>();

        var response = objResult.Value.As<ProblemDetails>();
        response.Should().NotBeNull();
        response.Status.Should().Be(expectedStatusCode);
        response.Detail.Should().Be(expectedErrors.First().Message);
        response.Extensions.Should().HaveCount(1);

        var ext = response.Extensions.First();
        ext.Key.Should().Be("errors");
        var errors = ext.Value as IDictionary<string, string>;
        errors.Should().NotBeNull();
        errors.Should().HaveCount(expectedErrors.Count());
        foreach (var error in expectedErrors)
        {
            errors.Should().ContainKey(error.Code);
            errors.Should().ContainValue(error.Message);
        }
    }

    public static void ShouldBeValidationProblemResult(
        this ActionResult<TestResponse> actionResult,
        IEnumerable<Error> expectedErrors)
    {
        actionResult.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<ObjectResult>();

        var objResult = actionResult.Result.As<ObjectResult>();
        objResult.Should().NotBeNull();
        objResult.Value.Should().BeOfType<ValidationProblemDetails>();

        var response = objResult.Value.As<ValidationProblemDetails>();
        response.Should().NotBeNull();

        var validations = response as HttpValidationProblemDetails;
        validations.Should().NotBeNull();
        validations!.Errors.Should().HaveCount(expectedErrors.Count());
        foreach (var error in expectedErrors)
        {
            validations.Errors.Should().ContainKey(error.Code);
        }
    }
}
