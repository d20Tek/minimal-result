//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Minimal.Result.UnitTests;

public sealed partial class ResultTTests
{
    [TestMethod]
    public void IfOrElse_OnlyCallsSuccessOperation_WhenIsSuccessTrue()
    {
        // arrange
        Result<TestEntity> result = CreateTestResult();
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = result.IfOrElse(
            val => DefaultSuccessOperation(out successOperationCalled),
            [ExcludeFromCodeCoverage] (errors) =>
                DefaultFailureOperation(out failureOperationCalled));

        // assert
        newResult.Should().Be("ok");
        successOperationCalled.Should().BeTrue();
        failureOperationCalled.Should().BeFalse();
    }

    [TestMethod]
    public void IfOrElse_OnlyCallsFailureOperation_WhenIsFailureTrue()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = result.IfOrElse(
            [ExcludeFromCodeCoverage] (val) => DefaultSuccessOperation(out successOperationCalled),
            errors => DefaultFailureOperation(out failureOperationCalled));

        // assert
        newResult.Should().Be("problem");
        successOperationCalled.Should().BeFalse();
        failureOperationCalled.Should().BeTrue();
    }

    [TestMethod]
    public void IfOrElseAction_OnlyCallsSuccessOperation_WhenIsSuccessTrue()
    {
        // arrange
        Result<TestEntity> result = CreateTestResult();
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        result.IfOrElse(
            val => DefaultSuccessAction(out successOperationCalled),
            [ExcludeFromCodeCoverage] (errors) => DefaultFailureAction(out failureOperationCalled));

        // assert
        successOperationCalled.Should().BeTrue();
        failureOperationCalled.Should().BeFalse();
    }

    [TestMethod]
    public void IfOrElseAction_OnlyCallsFailureOperation_WhenIsFailureTrue()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        result.IfOrElse(
            [ExcludeFromCodeCoverage] (val) => DefaultSuccessAction(out successOperationCalled),
            errors => DefaultFailureAction(out failureOperationCalled));

        // assert
        successOperationCalled.Should().BeFalse();
        failureOperationCalled.Should().BeTrue();
    }

    [TestMethod]
    public void IfOrElseAction_SkipsCallingFailureOperation_WhenFailureActionNull()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        result.IfOrElse([ExcludeFromCodeCoverage] (val) => DefaultSuccessAction(out successOperationCalled));

        // assert
        successOperationCalled.Should().BeFalse();
        failureOperationCalled.Should().BeFalse();
    }

    [TestMethod]
    public void IfOrElseResult_OnlyCallsSuccessOperation_WhenIsSuccessTrue()
    {
        // arrange
        Result<TestEntity> result = CreateTestResult();
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = result.IfOrElse(
            val => DefaultSuccessActionWithResult(val, ref successOperationCalled),
            [ExcludeFromCodeCoverage] (errors) => DefaultErrorActionWithResult(errors, ref failureOperationCalled));

        // assert
        newResult.IsSuccess.Should().BeTrue();
        successOperationCalled.Should().BeTrue();
        failureOperationCalled.Should().BeFalse();
    }

    [TestMethod]
    public void IfOrElse_OnlyCallsFailureOperation_WhenIsSuccessFalse()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = result.IfOrElse(
            [ExcludeFromCodeCoverage] (val) => DefaultSuccessActionWithResult(val, ref successOperationCalled),
            errors => DefaultErrorActionWithResult(errors, ref failureOperationCalled));

        // assert
        newResult.IsSuccess.Should().BeFalse();
        successOperationCalled.Should().BeFalse();
        failureOperationCalled.Should().BeTrue();
    }

    [TestMethod]
    public void IfOrElse_SkipsCallFailureOperation_WhenFailureOperationOmitted()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = result.IfOrElse(
            [ExcludeFromCodeCoverage] (val) => DefaultSuccessActionWithResult(val, ref successOperationCalled));

        // assert
        newResult.IsSuccess.Should().BeFalse();
        successOperationCalled.Should().BeFalse();
        failureOperationCalled.Should().BeFalse();
    }

    private Result<TestEntity> DefaultSuccessActionWithResult(TestEntity val, ref bool successActionCalled)
    {
        successActionCalled = true;
        return val;
    }

    private Result<TestEntity> DefaultErrorActionWithResult(IEnumerable<Error> errors, ref bool failureActionCalled)
    {
        failureActionCalled = true;
        return errors.ToArray();
    }
}
