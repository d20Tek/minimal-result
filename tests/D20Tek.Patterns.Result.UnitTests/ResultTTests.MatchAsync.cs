//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Patterns.Result.UnitTests;

public partial class ResultTTests
{
    [TestMethod]
    public async Task MatchAsync_OnlyCallsSuccessOperation_WhenIsSuccessTrue()
    {
        // arrange
        Result<TestEntity> result = CreateTestResult();
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = await result.MatchAsync(
            val => DefaultSuccessOperationAsync(out successOperationCalled),
            [ExcludeFromCodeCoverage] (errors) => default!);

        // assert
        newResult.Should().Be("ok");
        successOperationCalled.Should().BeTrue();
        failureOperationCalled.Should().BeFalse();
    }

    [TestMethod]
    public async Task MatchAsync_OnlyCallsFailureOperation_WhenIsFailureTrue()
    {
        // arrange
        Result<TestEntity> result = DefaultErrors.Conflict;
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = await result.MatchAsync(
            [ExcludeFromCodeCoverage] (val) => default!,
            errors => DefaultFailureOperationAsync(out failureOperationCalled));

        // assert
        newResult.Should().Be("problem");
        successOperationCalled.Should().BeFalse();
        failureOperationCalled.Should().BeTrue();
    }

    [TestMethod]
    public async Task MatchFirstErrorAsync_OnlyCallsSuccessOperation_WhenIsSuccessTrue()
    {
        // arrange
        Result<TestEntity> result = CreateTestResult();
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = await result.MatchFirstErrorAsync(
            val => DefaultSuccessOperationAsync(out successOperationCalled),
            [ExcludeFromCodeCoverage] (error) => default!);

        // assert
        newResult.Should().Be("ok");
        successOperationCalled.Should().BeTrue();
        failureOperationCalled.Should().BeFalse();
    }

    [TestMethod]
    public async Task MatchFirstErrorAsync_OnlyCallsFailureOperation_WhenIsFailureTrue()
    {
        // arrange
        Result<TestEntity> result = new Error[] { DefaultErrors.Conflict, DefaultErrors.Unauthorized };
        bool successOperationCalled = false, failureOperationCalled = false;

        // act
        var newResult = await result.MatchFirstErrorAsync(
            [ExcludeFromCodeCoverage] (val) => default!,
            error => DefaultFailureOperationAsync(error, out failureOperationCalled));

        // assert
        newResult.Should().Be("problem");
        successOperationCalled.Should().BeFalse();
        failureOperationCalled.Should().BeTrue();
    }

    private Task<string> DefaultSuccessOperationAsync(out bool successOperationCalled)
    {
        successOperationCalled = true;
        return Task.FromResult("ok");
    }

    private Task<string> DefaultFailureOperationAsync(Error error, out bool failureOperationCalled)
    {
        failureOperationCalled = error.Equals(DefaultErrors.Conflict);
        return Task.FromResult("problem");
    }

    private Task<string> DefaultFailureOperationAsync(out bool failureOperationCalled)
    {
        failureOperationCalled = true;
        return Task.FromResult("problem");
    }
}
