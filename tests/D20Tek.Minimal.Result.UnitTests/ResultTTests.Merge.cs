//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Minimal.Result.UnitTests;

public sealed partial class ResultTTests
{
    [TestMethod]
    public void Merge_MultipleSuccessChain_ReturnsIsSuccessTrue()
    {
        // arrange
        Result<int> initial = 42;

        // act
        var result = initial.Merge<string>(x => "test2")
                            .Merge<bool>(x => true)
                            .Merge<string>(x => "test succeeded");

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("test succeeded");
    }

    [TestMethod]
    public void Merge_FailureMidChain_ReturnsIsSuccessFalse()
    {
        // arrange
        Result<int> initial = 42;

        // act
        var result = initial.Merge<string>(x => "test2")
                            .Merge<bool>(x => Error.Invalid("Test.Failure", "Test failed as expected."))
                            .Merge<string>([ExcludeFromCodeCoverage] (x) => "test succeeded");

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.First().Code.Should().Be("Test.Failure");
    }

    [TestMethod]
    public void Merge_FirstFailureInChain_ReturnsIsSuccessFalse()
    {
        // arrange
        Result<int> initial = 42;

        // act
        var result = initial.Merge<string>(x => Error.Invalid("Test.Failure.1", "Test failed as expected."))
                            .Merge<bool>([ExcludeFromCodeCoverage] (x) => Error.Invalid("Test.Failure.2", "Test failed as expected."))
                            .Merge<string>([ExcludeFromCodeCoverage] (x) => Error.Invalid("Test.Failure.3", "Test failed as expected."));

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.First().Code.Should().Be("Test.Failure.1");
    }

    [TestMethod]
    public void Merge_InitialFailureSkipsChain_ReturnsIsSuccessFalse()
    {
        // arrange
        Result<int> initial = Error.Invalid("Test.Failure.Initial", "Test failed as expected.");

        // act
        var result = initial.Merge<string>([ExcludeFromCodeCoverage] (x) => Error.Invalid("Test.Failure.1", "Test failed as expected."))
                            .Merge<bool>([ExcludeFromCodeCoverage] (x) => Error.Invalid("Test.Failure.2", "Test failed as expected."))
                            .Merge<string>([ExcludeFromCodeCoverage] (x) => Error.Invalid("Test.Failure.3", "Test failed as expected."));

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.First().Code.Should().Be("Test.Failure.Initial");
    }

    [TestMethod]
    public async Task ContinueMergeWith_MultipleSuccessChain_ReturnsIsSuccessTrue()
    {
        // arrange
        Result<int> initial = 42;

        // act
        var result = await initial.Merge<int, string>(async (x) => { await Task.CompletedTask; return "test2"; })
                                  .ContinueMergeWith<string, bool>(async (x) => { await Task.CompletedTask; return true; })
                                  .ContinueMergeWith<bool, string>(async (x) => { await Task.CompletedTask; return "test succeeded"; });

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("test succeeded");
    }

    [TestMethod]
    public async Task ContinueMergeWith_FailureMidChain_ReturnsIsSuccessFalse()
    {
        // arrange
        Result<int> initial = 42;

        // act
        var result = await initial.Merge<int, string>(async (x) => { await Task.CompletedTask; return "test2"; })
                                  .ContinueMergeWith<string, bool>(async (x) => { await Task.CompletedTask; return Error.Invalid("Test.Failure", "Test failed as expected."); })
                                  .ContinueMergeWith<bool, string>(UnusedCheck);

        [ExcludeFromCodeCoverage]
        async Task<Result<string>> UnusedCheck(bool x)
        {
            await Task.CompletedTask;
            return "test succeeded";
        }

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.First().Code.Should().Be("Test.Failure");
    }

    [TestMethod]
    public async Task ContinueMergeWith_InitialFailureSkipsChain_ReturnsIsSuccessFalse()
    {
        // arrange
        Result<int> initial = Error.Invalid("Test.Failure.Initial", "Test failed as expected.");

        // act
        var result = await initial.Merge<int,string>(UnusedCheck1)
                                  .ContinueMergeWith<string, bool>(UnusedCheck2)
                                  .ContinueMergeWith<bool, string>(UnusedCheck3);

        [ExcludeFromCodeCoverage]
        async Task<Result<string>> UnusedCheck1(int x)
        {
            await Task.CompletedTask;
            return Error.Invalid("Test.Failure.1", "Test failed as expected.");
        }

        [ExcludeFromCodeCoverage]
        async Task<Result<bool>> UnusedCheck2(string x)
        {
            await Task.CompletedTask;
            return Error.Invalid("Test.Failure.2", "Test failed as expected.");
        }

        [ExcludeFromCodeCoverage]
        async Task<Result<string>> UnusedCheck3(bool x)
        {
            await Task.CompletedTask;
            return Error.Invalid("Test.Failure.2", "Test failed as expected.");
        }

        // assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Count.Should().Be(1);
        result.Errors.First().Code.Should().Be("Test.Failure.Initial");
    }

    [TestMethod]
    public void IfOrElseResult_WithSuccess_ReturnsSameResult()
    {
        // arrange
        Result<int> initial = 42;
        bool failureActionCalled = false;

        // act
        var result = initial.IfOrElseResult(
            DefaultAction,
            [ExcludeFromCodeCoverage] (e) => failureActionCalled = true);

        void DefaultAction<T>(T x)
        {
            Console.WriteLine($"{0}", x);
        }

        // assert
        result.IsSuccess.Should().BeTrue();
        failureActionCalled.Should().BeFalse();
    }

    [TestMethod]
    public void IfOrElseResult_WithIsFaliureButNoAction_ReturnsSameResult()
    {
        // arrange
        Result<int> initial = Error.Invalid("Failure", "Test error.");

        // act
        var result = initial.IfOrElseResult(DefaultAction);

        [ExcludeFromCodeCoverage]
        void DefaultAction<T>(T x)
        {
            Console.WriteLine($"{0}", x);
        }

        // assert
        result.IsSuccess.Should().BeFalse();
    }

    [TestMethod]
    public void IfOrElseResult_WithFailure_ReturnsSameResult()
    {
        // arrange
        Result<int> initial = Error.Invalid("Failure", "Test error.");
        bool failureActionCalled = false;

        // act
        var result = initial.IfOrElseResult(DefaultAction, x => failureActionCalled = true);

        [ExcludeFromCodeCoverage]
        void DefaultAction<T>(T x)
        {
            Console.WriteLine($"{0}", x);
        }

        // assert
        result.IsSuccess.Should().BeFalse();
        failureActionCalled.Should().BeTrue();
    }
}
