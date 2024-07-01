//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Minimal.Result.UnitTests;

public sealed partial class ResultTTests
{
    [TestMethod]
    public void Match_OnlyCallsSuccessOperation_WhenIsSuccessTrue()
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
    public void Match_OnlyCallsFailureOperation_WhenIsFailureTrue()
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
    public void MatchAction_OnlyCallsSuccessOperation_WhenIsSuccessTrue()
    {
        // arrange
        Result<TestEntity> result = CreateTestResult();
        bool successActionCalled = false, failureActionCalled = false;

        // act
        result.IfOrElse(
            val => DefaultSuccessAction(out successActionCalled),
            [ExcludeFromCodeCoverage] (errors) => DefaultFailureAction(out failureActionCalled));

        // assert
        successActionCalled.Should().BeTrue();
        failureActionCalled.Should().BeFalse();
    }

    [TestMethod]
    public void MatchAction_OnlyCallsFailureOperation_WhenIsFailureTrue()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;
        bool successActionCalled = false, failureActionCalled = false;

        // act
        result.IfOrElse(
            [ExcludeFromCodeCoverage] (val) => DefaultSuccessAction(out successActionCalled),
            errors => DefaultFailureAction(out failureActionCalled));

        // assert
        successActionCalled.Should().BeFalse();
        failureActionCalled.Should().BeTrue();
    }

    [TestMethod]
    public void MatchAction_SkipsCallingFailureOperation_WhenFailureActionNull()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;
        bool successActionCalled = false, failureActionCalled = false;

        // act
        result.IfOrElse([ExcludeFromCodeCoverage] (val) => DefaultSuccessAction(out successActionCalled));

        // assert
        successActionCalled.Should().BeFalse();
        failureActionCalled.Should().BeFalse();
    }

    [TestMethod]
    public void IfOrElse_OnlyCallsSuccessOperation_WhenIsSuccessTrue()
    {
        // arrange
        Result<TestEntity> result = CreateTestResult();
        bool successActionCalled = false, failureActionCalled = false;

        // act
        var newResult = result.IfOrElse(
            val => DefaultSuccessActionWithResult(val, ref successActionCalled),
            [ExcludeFromCodeCoverage] (errors) => DefaultErrorActionWithResult(errors, ref failureActionCalled));

        // assert
        newResult.IsSuccess.Should().BeTrue();
        successActionCalled.Should().BeTrue();
        failureActionCalled.Should().BeFalse();
    }

    [TestMethod]
    public void IfOrElse_OnlyCallsFailureOperation_WhenIsSuccessFalse()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;
        bool successActionCalled = false, failureActionCalled = false;

        // act
        var newResult = result.IfOrElse(
            [ExcludeFromCodeCoverage] (val) => DefaultSuccessActionWithResult(val, ref successActionCalled),
            errors => DefaultErrorActionWithResult(errors, ref failureActionCalled));

        // assert
        newResult.IsSuccess.Should().BeFalse();
        successActionCalled.Should().BeFalse();
        failureActionCalled.Should().BeTrue();
    }

    [TestMethod]
    public void IfOrElse_SkipsCallFailureOperation_WhenFailureOperationOmitted()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;
        bool successActionCalled = false, failureActionCalled = false;

        // act
        var newResult = result.IfOrElse(
            [ExcludeFromCodeCoverage] (val) => DefaultSuccessActionWithResult(val, ref successActionCalled));

        // assert
        newResult.IsSuccess.Should().BeFalse();
        successActionCalled.Should().BeFalse();
        failureActionCalled.Should().BeFalse();
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
