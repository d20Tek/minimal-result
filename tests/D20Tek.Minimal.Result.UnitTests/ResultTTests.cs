﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result.UnitTests.Assertions;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Minimal.Result.UnitTests;

[TestClass]
public sealed partial class ResultTTests
{
    [ExcludeFromCodeCoverage]
    public class TestEntity
    {
        public int Code { get; set; }

        public string Message { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public record TestResponse(int Status, string Details);

    [TestMethod]
    public void ImplicitSuccess_ReturnsIsSuccess_True()
    {
        // arrange

        // act
        Result<int> result = 101;

        // assert
        result.ShouldBeSuccess();
        result.Value.GetType().Should().Be(typeof(int));
        result.Value.Should().Be(101);
    }

    [TestMethod]
    public void ImplicitError_ReturnsIsFailure_True()
    {
        // arrange

        // act
        Result<TestEntity> result = DefaultErrors.NotFound;

        // assert
        result.ShouldBeFailure(DefaultErrors.NotFound);
    }

    [TestMethod]
    public void ImplicitErrors_ReturnsIsFailure_True()
    {
        // arrange
        var errors = new[] { DefaultErrors.NotFound, DefaultErrors.Conflict };

        // act
        Result<TestEntity> result = errors;

        // assert
        result.ShouldBeFailure(DefaultErrors.NotFound, DefaultErrors.Conflict);
    }

    [TestMethod]
    public void ImplicitErrorsList_ReturnsIsFailure_True()
    {
        // arrange
        var errors = new List<Error> { DefaultErrors.NotFound, DefaultErrors.Conflict };

        // act
        Result<TestEntity> result = errors;

        // assert
        result.ShouldBeFailure(DefaultErrors.NotFound, DefaultErrors.Conflict);
    }

    [TestMethod]
    public void ImplicitException_ReturnsIsFailure_True()
    {
        // arrange
        var ex = new ArgumentOutOfRangeException("Test exception.");
        var expected = Error.Custom("General.Exception", ex.Message, ErrorType.Unexpected);

        // act
        Result<TestEntity> result = ex;

        // assert
        result.ShouldBeFailure(expected);
    }

    [TestMethod]
    public void Success_CreatesResultWithValue()
    {
        // arrange

        // act
        var result = Result<int>.Success(101);

        // assert
        result.ShouldBeSuccess();
        result.Value.GetType().Should().Be(typeof(int));
        result.Value.Should().Be(101);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    [ExcludeFromCodeCoverage]
    public void Result_WithFailure_HasNullValue()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.NotFound;

        // act
        _ = result.Value;

        // assert
    }

    [TestMethod]
    public void MapResult_WithSuccess()
    {
        // arrange
        var entity = new TestEntity { Code = 1001, Message = "another messge" };
        Result<TestEntity> result = entity;

        // act
        var mappedResult = result.MapResult(MapEntity);

        // assert
        mappedResult.ShouldBeSuccess();
        mappedResult.Value.Should().BeOfType<TestResponse>();
        var value = mappedResult.Value.As<TestResponse>();
        value.Should().NotBeNull();
        value.Status.Should().Be(entity.Code);
        value.Details.Should().Be(entity.Message);
    }

    [TestMethod]
    public void MapResult_WithFailure()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Unexpected;

        // act
        var mappedResult = result.MapResult(MapEntity);

        // assert
        mappedResult.ShouldBeFailure();
    }

    [TestMethod]
    public void ToErrorResult_WithFailure()
    {
        // arrange
        Result<int> start = DefaultErrors.Unexpected;

        // act
        var result = start.ToErrorResult<TestEntity>();

        // assert
        result.ShouldBeFailure();
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ToErrorResult_WithSuccess_ThrowsException()
    {
        // arrange
        Result<int> start = 42;

        // act
        _ = start.ToErrorResult<TestEntity>();

        // assert
    }

    private TestResponse MapEntity(TestEntity entity) =>
        new TestResponse(entity.Code, entity.Message);

    private string DefaultSuccessOperation(out bool successOperationCalled)
    {
        successOperationCalled = true;
        return "ok";
    }

    private string DefaultFailureOperation(out bool failureOperationCalled)
    {
        failureOperationCalled = true;
        return "problem";
    }

    private void DefaultSuccessAction(out bool successActionCalled)
    {
        successActionCalled = true;
    }

    private void DefaultFailureAction(out bool failureActionCalled)
    {
        failureActionCalled = true;
    }

    private Result<TestEntity> CreateTestResult()
    {
        var entity = new TestEntity { Code = 42, Message = "test message" };
        Result<TestEntity> result = entity;
        return result;
    }
}
