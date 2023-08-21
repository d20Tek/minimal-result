//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result.AspNetCore.WebApi;
using D20Tek.Minimal.Result.UnitTests.Assertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace D20Tek.Minimal.Result.UnitTests.WebApi;

[TestClass]
public sealed class ResultExtensionsTests
{
    private readonly TestController _controller = new TestController();

    [TestMethod]
    public void ToActionResult_WithMappingFunc_ReturnsOK()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(42, "ActionTest", DateTime.UtcNow);

        // act
        var actionResult = modelResult.ToActionResult(ToResponse, _controller);

        // assert
        actionResult.ShouldBeOkResult(42, "ActionTest");
    }

    [TestMethod]
    public void ToActionResult_WithMappingFunc_ReturnsNotFound()
    {
        // arrange
        var error = Error.Custom("Test.Error", "Test error message", ErrorType.NotFound);
        Result<TestEntity> modelResult = error;

        // act
        var actionResult = modelResult.ToActionResult(ToResponse, _controller);

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status404NotFound, error);
    }

    [TestMethod]
    public void ToActionResult_WithResponse_ReturnsOK()
    {
        // arrange
        var entity = new TestEntity(101, "Test", DateTime.UtcNow);
        Result<TestEntity> modelResult = entity;

        // act
        var converted = ToResponse(entity);
        var actionResult = modelResult.ToActionResult(converted, _controller);

        // assert
        actionResult.ShouldBeOkResult(101, "Test");
    }

    [TestMethod]
    public void ToActionResult_WithResponse_ReturnsBadRequest()
    {
        // arrange
        var error = Error.Custom("Test.Error", "Test error message", ErrorType.Failure);
        Result<TestEntity> modelResult = error;

        // act
        var actionResult = modelResult.ToActionResult(new TestResponse(1, "foo"), _controller);

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status400BadRequest, error);
    }

    [TestMethod]
    public void ToActionResult_WithUntypedValue_ReturnsOK()
    {
        // arrange
        Result modelResult = Result.Success();

        // act
        var actionResult = modelResult.ToActionResult<TestResponse>(_controller);

        // assert
        actionResult.Should().NotBeNull();
        actionResult.Result.Should().BeOfType<OkResult>();
        
        var okResult = actionResult.Result.As<OkResult>();
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [TestMethod]
    public void ToActionResult_WithUntypedValue_ReturnsUnauthorized()
    {
        // arrange
        var error = Error.Custom("Test.Error", "Test error message", ErrorType.Unauthorized);
        Result modelResult = error;

        // act
        var actionResult = modelResult.ToActionResult<TestResponse>(_controller);

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status401Unauthorized, error);
    }

    [TestMethod]
    public void ToActionResult_WithMultipleErrors_ReturnsFirstAndErrorList()
    {
        // arrange
        var errors = new List<Error>
        {
            Error.Custom("Test.Error", "Test error message", ErrorType.Unauthorized),
            Error.Validation("foo", "bar"),
            DefaultErrors.Conflict
        };
        Result<TestEntity> modelResult = errors;

        // act
        var actionResult = modelResult.ToActionResult(ToResponse, _controller);

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status401Unauthorized, errors);
    }

    [TestMethod]
    public void ToCreatedActionResult_WithMappingFuncAndRouteName_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(201, "Test2", DateTime.UtcNow);

        // act
        var actionResult = modelResult.ToCreatedActionResult(
            ToResponse,
            _controller,
            "CreateTest",
            new { id = 201 });

        // assert
        actionResult.ShouldBeCreatedAtResult(201, "Test2");
    }

    [TestMethod]
    public void ToCreatedActionResult_WithMappingFuncAndRouteName_ReturnsConflict()
    {
        // arrange
        var error = Error.Custom("Test.Error", "Test error message", ErrorType.Conflict);
        Result<TestEntity> modelResult = error;

        // act
        var apiResult = modelResult.ToCreatedActionResult(
            ToResponse,
            _controller,
            "CreateTest",
            new { id = 201 });

        // assert
        apiResult.ShouldBeProblemResult(StatusCodes.Status409Conflict, error);
    }

    [TestMethod]
    public void ToCreatedApiResult_WithResponseAndRouteName_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(201, "Test2", DateTime.UtcNow);
        var converted = new TestResponse(201, "Test2");

        // act
        var actionResult = modelResult.ToCreatedActionResult(
            converted,
            _controller,
            "CreateTest",
            new { id = 201 });

        // assert
        actionResult.ShouldBeCreatedAtResult(201, "Test2");
    }

    [TestMethod]
    public void ToCreatedApiResult_WithResponseAndRouteName_ReturnsUnprocessableEntity()
    {
        // arrange
        var error = Error.Custom("Test.Error", "Test error message", ErrorType.Invalid);
        Result<TestEntity> modelResult = error;
        var converted = new TestResponse(201, "Test2");

        // act
        var actionResult = modelResult.ToCreatedActionResult(
            converted,
            _controller,
            "CreateTest",
            new { id = 201 });

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status422UnprocessableEntity, error);
    }

    [TestMethod]
    public void ToCreatedActionResult_WithMappingFuncAndRouteUri_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(301, "Test3", DateTime.UtcNow);

        // act
        var actionResult = modelResult.ToCreatedActionResult(ToResponse, _controller, "/tests/301");

        // assert
        actionResult.ShouldBeCreatedResult("/tests/301", 301, "Test3");
    }

    [TestMethod]
    public void ToCreatedActionResult_WithMappingFuncAndRouteUri_ReturnsUnexpected()
    {
        // arrange
        var error = Error.Custom("Test.Error", "Test error message", ErrorType.Unexpected);
        Result<TestEntity> modelResult = error;

        // act
        var actionResult = modelResult.ToCreatedActionResult(ToResponse, _controller, "/tests/301");

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status500InternalServerError, error);
    }

    [TestMethod]
    public void ToCreatedActionResult_WithResponseAndRouteUri_ReturnsCreated()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(301, "Test3", DateTime.UtcNow);
        var converted = new TestResponse(301, "Test3");

        // act
        var actionResult = modelResult.ToCreatedActionResult(converted, _controller, "/tests/301");

        // assert
        actionResult.ShouldBeCreatedResult("/tests/301", 301, "Test3");
    }

    [TestMethod]
    public void ToCreatedActionResult_WithResponseAndRouteUri_ReturnsForbidden()
    {
        // arrange
        var error = Error.Custom("Test.Error", "Test error message", ErrorType.Forbidden);
        Result<TestEntity> modelResult = error;
        var converted = new TestResponse(301, "Test3");

        // act
        var actionResult = modelResult.ToCreatedActionResult(converted, _controller, "/tests/301");

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status403Forbidden, error);
    }

    [TestMethod]
    public void ToIActionResult_WithMappingFunc_ReturnsOK()
    {
        // arrange
        Result<TestEntity> modelResult = new TestEntity(42, "ActionTest", DateTime.UtcNow);

        // act
        var actionResult = modelResult.ToActionResult(ToResponse, _controller).ToIActionResult();

        // assert
        actionResult.ShouldBeOkResult(42, "ActionTest");
    }

    [TestMethod]
    public void ToIActionResult_WithMappingFunc_ReturnsNotFound()
    {
        // arrange
        var error = Error.Custom("Test.Error", "Test error message", ErrorType.NotFound);
        Result<TestEntity> modelResult = error;

        // act
        var actionResult = modelResult.ToActionResult(ToResponse, _controller).ToIActionResult();

        // assert
        actionResult.ShouldBeProblemResult(StatusCodes.Status404NotFound, error);
    }

    private TestResponse ToResponse(TestEntity entity)
    {
        return new TestResponse(entity.Id, entity.Message);
    }
}
