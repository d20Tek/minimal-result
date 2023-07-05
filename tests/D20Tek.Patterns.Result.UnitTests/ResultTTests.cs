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
    [ExpectedException(typeof(InvalidOperationException))]
    [ExcludeFromCodeCoverage]
    public void Result_WithFailure_ValueThrowsException()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.NotFound;

        // act
        _ = result.Value;

        // assert
    }

    [TestMethod]
    public void Match_OnlyCallsSuccessOperation_WhenIsSuccessTrue()
    {
        // arrange
        Result<TestEntity> result = CreateTestResult();
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = result.Match(
            val => DefaultSuccessOperation(out successOperationCalled),
            errors => DefaultFailureOperation(out failureOperationCalled));

        // assert
        newResult.Should().Be("ok");
        successOperationCalled.Should().BeTrue();
        failureOperationCalled.Should().BeFalse();
    }

    [TestMethod]
    public void Match_OnlyCallsFailureOperation_WhenIsFailureTrue()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = result.Match(
            val => DefaultSuccessOperation(out successOperationCalled),
            errors => DefaultFailureOperation(out failureOperationCalled));

        // assert
        newResult.Should().Be("problem");
        successOperationCalled.Should().BeFalse();
        failureOperationCalled.Should().BeTrue();
    }

    [TestMethod]
    public void MatchFirstError_OnlyCallsSuccessOperation_WhenIsSuccessTrue()
    {
        // arrange
        Result<TestEntity> result = CreateTestResult();
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = result.MatchFirstError(
            val => DefaultSuccessOperation(out successOperationCalled),
            error => DefaultFailureOperation(error, out failureOperationCalled));

        // assert
        newResult.Should().Be("ok");
        successOperationCalled.Should().BeTrue();
        failureOperationCalled.Should().BeFalse();
    }

    [TestMethod]
    public void MatchFirstError_OnlyCallsFailureOperation_WhenIsFailureTrue()
    {
        // arrange
        Result<TestEntity> result = new Error[] { DefaultErrors.Conflict, DefaultErrors.Unauthorized };
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = result.MatchFirstError(
            val => DefaultSuccessOperation(out successOperationCalled),
            error => DefaultFailureOperation(error, out failureOperationCalled));

        // assert
        newResult.Should().Be("problem");
        successOperationCalled.Should().BeFalse();
        failureOperationCalled.Should().BeTrue();
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

    private Result<TestEntity> CreateTestResult()
    {
        var entity = new TestEntity { Code = 42, Message = "test message" };
        Result<TestEntity> result = entity;
        return result;
    }
}
