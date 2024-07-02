//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Minimal.Result.UnitTests;

public sealed partial class ResultTTests
{
    [TestMethod]
    public async Task IfOrElseAsync_OnlyCallsSuccessOperation_WhenIsSuccessTrue()
    {
        // arrange
        Result<TestEntity> result = CreateTestResult();
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = await result.IfOrElse(
            val => DefaultSuccessOperationAsync(out successOperationCalled),
            [ExcludeFromCodeCoverage] (errors) => default!);

        // assert
        newResult.Should().Be("ok");
        successOperationCalled.Should().BeTrue();
        failureOperationCalled.Should().BeFalse();
    }

    [TestMethod]
    public async Task IfOrElseAsync_OnlyCallsFailureOperation_WhenIsFailureTrue()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = await result.IfOrElse(
            [ExcludeFromCodeCoverage] (val) => default!,
            errors => DefaultFailureOperationAsync(out failureOperationCalled));

        // assert
        newResult.Should().Be("problem");
        successOperationCalled.Should().BeFalse();
        failureOperationCalled.Should().BeTrue();
    }

    [TestMethod]
    public async Task IfOrElseActionAsync_OnlyCallsSuccessAction_WhenIsSuccessTrue()
    {
        // arrange
        Result<TestEntity> result = CreateTestResult();
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        await result.IfOrElse(
            val => DefaultSuccessActionAsync(out successOperationCalled),
            [ExcludeFromCodeCoverage] (errors) => default!);

        // assert
        successOperationCalled.Should().BeTrue();
        failureOperationCalled.Should().BeFalse();
    }

    [TestMethod]
    public async Task IfOrElseActionAsync_OnlyCallsFailureAction_WhenIsFailureTrue()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        await result.IfOrElse(
            [ExcludeFromCodeCoverage] (val) => default!,
            errors => DefaultFailureActionAsync(out failureOperationCalled));

        // assert
        successOperationCalled.Should().BeFalse();
        failureOperationCalled.Should().BeTrue();
    }

    [TestMethod]
    public async Task IfOrElseAsyncResult_OnlyCallsSuccessOperation_WhenIsSuccessTrue()
    {
        // arrange
        Result<TestEntity> result = CreateTestResult();

        // act
        var newResult = await result.IfOrElseResult<TestEntity>(
            async val => await DefaultSuccessActionWithResultAsync(val),
            [ExcludeFromCodeCoverage] async (errors) => await DefaultErrorActionWithResultAsync(errors));

        // assert
        newResult.IsSuccess.Should().BeTrue();
    }

    [TestMethod]
    public async Task IfOrElseAsync_OnlyCallsFailureOperation_WhenIsSuccessFalse()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;

        // act
        var newResult = await result.IfOrElseResult<TestEntity>(
            [ExcludeFromCodeCoverage] async (val) => await DefaultSuccessActionWithResultAsync(val),
            async (errors) => await DefaultErrorActionWithResultAsync(errors));

        // assert
        newResult.IsSuccess.Should().BeFalse();
    }

    [TestMethod]
    public async Task IfOrElseAsync_SkipsCallFailureOperation_WhenFailureOperationOmitted()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;

        // act
        var newResult = await result.IfOrElseResult<TestEntity>(
            [ExcludeFromCodeCoverage] async (val) => await DefaultSuccessActionWithResultAsync(val));

        // assert
        newResult.IsSuccess.Should().BeFalse();
    }

    private Task<string> DefaultSuccessOperationAsync(out bool successOperationCalled)
    {
        successOperationCalled = true;
        return Task.FromResult("ok");
    }

    private Task DefaultSuccessActionAsync(out bool successOperationCalled)
    {
        successOperationCalled = true;
        return Task.CompletedTask;
    }

    private Task<string> DefaultFailureOperationAsync(out bool failureOperationCalled)
    {
        failureOperationCalled = true;
        return Task.FromResult("problem");
    }

    private Task DefaultFailureActionAsync(out bool failureOperationCalled)
    {
        failureOperationCalled = true;
        return Task.CompletedTask;
    }

    private async Task<Result<TestEntity>> DefaultSuccessActionWithResultAsync(TestEntity val)
    {
        await Task.CompletedTask;
        return val;
    }

    private async Task<Result<TestEntity>> DefaultErrorActionWithResultAsync(IEnumerable<Error> errors)
    {
        await Task.CompletedTask;
        return errors.ToArray();
    }
}
