//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result.Client;
using D20Tek.Minimal.Result.UnitTests.Assertions;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace D20Tek.Minimal.Result.UnitTests.Client;

[TestClass]
public class ProblemDetailsTests
{
    [TestMethod]
    public void Create_ProblemDetails_ResultsObject()
    {
        // arrange

        // act
        var problem = new ProblemDetails
        {
            Type = "testType",
            Title = "Test 1",
            Status = 404,
            Detail = "Not Found",
            Instance = "/test/not-found",
            Errors = new Dictionary<string, object?> {
                { "Error.NotFound", "Element not found" }
            },
            Extensions = new Dictionary<string, object?> { }
        };

        // assert
        problem.Should().NotBeNull();
        problem.Type.Should().Be("testType");
        problem.Title.Should().Be("Test 1");
        problem.Status.Should().Be(404);
        problem.Detail.Should().Be("Not Found");
        problem.Instance.Should().Be("/test/not-found");
        problem.Errors.Should().HaveCount(1);
        problem.Extensions.Should().BeEmpty();
    }

    [TestMethod]
    public void Create_EmptyProblemDetails_ResultsObject()
    {
        // arrange

        // act
        var problem = new ProblemDetails();

        // assert
        problem.Should().NotBeNull();
        problem.Type.Should().BeNull();
        problem.Title.Should().BeNull();
        problem.Status.Should().Be(500);
        problem.Detail.Should().BeNull();
        problem.Instance.Should().BeNull();
        problem.Errors.Should().BeNullOrEmpty();
        problem.Extensions.Should().BeNullOrEmpty();
    }

    [TestMethod]
    public void ToResult_WithProblemDetailsAndMultipleErrors_ReturnsResult()
    {
        // arrange
        var problem = new ProblemDetails
        {
            Type = "testType",
            Title = "Test 2",
            Status = 400,
            Detail = "Validations failed",
            Instance = "/test/bad-request",
            Errors = new Dictionary<string, object?> {
                { "Error.NameEmpty",  CreateJsonStringElement("\"Element name cannot be empty. }\"") },
                { "Error.DescriptionEmpty", CreateJsonStringElement("\"Element description cannot be empty.\"") },
                { "Error.InvalidEmail", CreateJsonStringElement("\"Element email address is an invalid format.\"") }
            },
            Extensions = new Dictionary<string, object?> { }
        };

        // act
        var result = problem.ToResult<TestValue>();

        // assert
        result.ShouldBeFailure();
    }

    [TestMethod]
    public void ToResult_WithProblemDetailsAndMultipleErrorArrays_ReturnsResult()
    {
        // arrange
        var problem = new ProblemDetails
        {
            Type = "testType",
            Title = "Test 2",
            Status = null,
            Detail = "Validations failed",
            Instance = "/test/bad-request",
            Errors = new Dictionary<string, object?> {
                { "Error.NameEmpty",  CreateJsonStringElement("[ \"Element name cannot be empty. }\" ]") },
                { "Error.DescriptionEmpty", CreateJsonStringElement("[ \"Element description cannot be empty.\" ]") },
                { "Error.InvalidEmail", CreateJsonStringElement("[ \"Element email address is an invalid format.\" ]") }
            },
            Extensions = new Dictionary<string, object?> { }
        };

        // act
        var result = problem.ToResult<TestValue>();

        // assert
        result.ShouldBeFailure();
    }

    [TestMethod]
    public void ToResult_WithProblemDetailsAndNullError_ReturnsResult()
    {
        // arrange
        var problem = new ProblemDetails
        {
            Type = "testType",
            Title = "Test 2",
            Detail = "Validations failed",
            Instance = "/test/bad-request",
            Errors = new Dictionary<string, object?> {
                { "Error.NameEmpty",  CreateJsonStringElement("[ null ]") },
            },
            Extensions = new Dictionary<string, object?> { }
        };

        // act
        var result = problem.ToResult<TestValue>();

        // assert
        result.ShouldBeFailure();
    }

    private JsonElement CreateJsonStringElement(string text)
    {
        JsonDocument document = JsonDocument.Parse(text);
        return document.RootElement;
    }

    [ExcludeFromCodeCoverage]
    private class TestValue
    {
        public string Name { get; set; } = default!;
    }
}
