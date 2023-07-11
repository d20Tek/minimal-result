//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result.UnitTests.Assertions;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Patterns.Result.UnitTests;

[TestClass]
public partial class ResultTTests
{
    [ExcludeFromCodeCoverage]
    public class TestEntity
    {
        public int Code { get; set; }

        public string Message { get; set; } = string.Empty;
    }

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
        var expected = Error.Custom("General.UnhandledException", ex.Message, ErrorType.Unexpected);

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
    public void Result_WithFailure_HasNullValue()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.NotFound;

        // act
        var value = result.Value;

        // assert
        value.Should().BeNull();
    }

    private string DefaultSuccessOperation(out bool successOperationCalled)
    {
        successOperationCalled = true;
        return "ok";
    }

    private string DefaultFailureOperation(Error error, out bool failureOperationCalled)
    {
        failureOperationCalled = error.Equals(DefaultErrors.Conflict);
        return "problem";
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

    private void DefaultFailureAction(Error error, out bool failureActionCalled)
    {
        failureActionCalled = error.Equals(DefaultErrors.Conflict);
    }

    private Result<TestEntity> CreateTestResult()
    {
        var entity = new TestEntity { Code = 42, Message = "test message" };
        Result<TestEntity> result = entity;
        return result;
    }
}
