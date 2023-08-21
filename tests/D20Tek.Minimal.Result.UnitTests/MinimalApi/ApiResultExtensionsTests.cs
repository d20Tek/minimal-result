//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result.AspNetCore.MinimalApi;
using D20Tek.Minimal.Result.UnitTests.Assertions;
using Microsoft.AspNetCore.Http;

namespace D20Tek.Minimal.Result.UnitTests.MinimalApi;

[TestClass]
public sealed class ApiResultExtensionsTests
{
    [TestMethod]
    public void Problem_WithEmptyErrors_ShouldProduceDefaultProblemDetails()
    {
        // arrange
        var errors = new List<Error>();

        // act
        var result = Results.Extensions.Problem(errors);

        // assert
        result.ShouldBeProblemResult(
            StatusCodes.Status500InternalServerError,
            "An error occurred while processing your request.");
    }

    [TestMethod]
    public void Problem_WithValidationErrors_ShouldProduceProblemDetailsWithMultipleItems()
    {
        // arrange
        var errors = new List<Error>
        {
            Error.Validation("Test.Id.Missing", "Id error."),
            Error.Validation("Test.Name.Empty", "Name is required.")
        };

        // act
        var result = Results.Extensions.Problem(errors);

        // assert
        result.ShouldBeValidationProblemResult(
            StatusCodes.Status400BadRequest,
            "Bad Request",
            errors);
    }

    [TestMethod]
    public void Problem_WithSingleError_ShouldProduceProblemDetails()
    {
        // arrange
        var error = DefaultErrors.Conflict;

        // act
        var result = Results.Extensions.Problem(error);

        // assert
        result.ShouldBeProblemResult(StatusCodes.Status409Conflict, "Conflict", error);
    }

    [TestMethod]
    public void Problem_WithSingleValidationError_ShouldProduceProblemDetails()
    {
        // arrange
        var error = Error.Validation("Test.Name.Empty", "Name is required.");

        // act
        var result = Results.Extensions.Problem(error);

        // assert
        result.ShouldBeValidationProblemResult(
            StatusCodes.Status400BadRequest,
            "Bad Request",
            new List<Error> { error });
    }

    [TestMethod]
    public void Problem_WithDecomposedError_ShouldProduceProblemDetails()
    {
        // arrange
        var expectedError = Error.Custom(
            "Custom.Code",
            "Custom test error.",
            StatusCodes.Status405MethodNotAllowed);

        // act
        var result = Results.Extensions.Problem(
            StatusCodes.Status405MethodNotAllowed,
            "Custom.Code",
            "Custom test error.");

        // assert
        result.ShouldBeProblemResult(
            StatusCodes.Status405MethodNotAllowed,
            "Method Not Allowed",
            expectedError);
    }

    [TestMethod]
    public void Problem_WithMixedValidationErrors_ShouldProduceProblemDetailsWithOneItem()
    {
        // arrange
        var errors = new List<Error>
        {
            Error.Validation("Test.Id.Missing", "Id error."),
            DefaultErrors.Unexpected
        };

        // act
        var result = Results.Extensions.Problem(errors);

        // assert
        result.ShouldBeProblemResult(
            StatusCodes.Status400BadRequest,
            "Bad Request",
            errors);
    }
}
