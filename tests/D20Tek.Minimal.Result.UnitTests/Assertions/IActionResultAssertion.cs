//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Minimal.Result.UnitTests.Assertions;

internal static class IActionResultAssertion
{
    public static void ShouldBeOkResult(
        this IActionResult actionResult,
        int expectedId,
        string expecteMessage)
    {
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType<OkObjectResult>();

        var okResult = actionResult.As<OkObjectResult>();
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Should().BeOfType<TestResponse>();

        var value = okResult.Value.As<TestResponse>();
        value.Should().NotBeNull();
        value!.Id.Should().Be(expectedId);
        value!.Message.Should().Be(expecteMessage);
    }

    public static void ShouldBeProblemResult(
        this IActionResult actionResult,
        int expectedStatusCode,
        Error expectedError)
    {
        actionResult.Should().NotBeNull();
        actionResult.Should().BeOfType<ObjectResult>();

        var objResult = actionResult.As<ObjectResult>();
        objResult.Should().NotBeNull();
        objResult.StatusCode.Should().Be(expectedStatusCode);
        objResult.Value.Should().BeOfType<ProblemDetails>();

        var response = objResult.Value.As<ProblemDetails>();
        response.Should().NotBeNull();
        response.Status.Should().Be(expectedStatusCode);
        response.Detail.Should().Be(expectedError.Message);

        response.Extensions.Should().HaveCount(1);

        var ext = response.Extensions.First();
        ext.Key.Should().Be("errors");
        var errors = ext.Value as IDictionary<string, string>;
        errors.Should().NotBeNull();
        errors.Should().HaveCount(1);
        errors.Should().ContainKey(expectedError.Code);
        errors.Should().ContainValue(expectedError.Message);
    }
}
