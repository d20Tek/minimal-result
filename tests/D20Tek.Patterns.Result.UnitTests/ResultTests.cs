//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result.UnitTests.Assertions;
using FluentAssertions;

namespace D20Tek.Patterns.Result.UnitTests;

[TestClass]
public class ResultTests
{
    [TestMethod]
    public void ImplicitSuccess_ReturnsIsSuccess_True()
    {
        // arrange

        // act
        var result = Result.Success();

        // assert
        result.ShouldBeSuccess();
    }

    [TestMethod]
    public void ImplicitError_ReturnsIsFailure_True()
    {
        // arrange

        // act
        Result result = DefaultErrors.NotFound;

        // assert
        result.ShouldBeFailure(DefaultErrors.NotFound);
    }

    [TestMethod]
    public void ImplicitErrors_ReturnsIsFailure_True()
    {
        // arrange
        var errors = new[] { DefaultErrors.NotFound, DefaultErrors.Conflict };

        // act
        Result result = errors;

        // assert
        result.ShouldBeFailure(DefaultErrors.NotFound, DefaultErrors.Conflict);
    }

    [TestMethod]
    public void ImplicitException_ReturnsIsFailure_True()
    {
        // arrange
        var msg = "test exception message.";
        var ex = new ArgumentException(msg);
        var expectedError = Error.Unexpected("General.UnhandledException", msg);

        // act
        Result result = ex;

        // assert
        result.ShouldBeFailure(expectedError);
    }

    [TestMethod]
    public void IfFailure_SkipsOperationCall_WhenResultSuccess()
    {
        // arrange
        var result = Result.Success();
        var operationCalled = false;

        // act
        result.IfFailure(errors => { operationCalled = true; });

        // assert
        operationCalled.Should().BeFalse();
    }

    [TestMethod]
    public void IfFailure_CallsOperation_WhenResultFailure()
    {
        // arrange
        Result result = DefaultErrors.Conflict;
        var operationCalled = false;

        // act
        result.IfFailure(errors => { operationCalled = true; });

        // assert
        operationCalled.Should().BeTrue();
    }

    [TestMethod]
    public async Task IfFailureAsync_SkipsOperationCall_WhenResultSuccess()
    {
        // arrange
        var result = Result.Success();
        var operationCalled = false;

        // act
        await result.IfFailureAsync(errors =>
        { 
            operationCalled = true;
            return Task.CompletedTask;
        });

        // assert
        operationCalled.Should().BeFalse();
    }

    [TestMethod]
    public async Task IfFailureAsync_CallsOperation_WhenResultFailure()
    {
        // arrange
        Result result = DefaultErrors.Conflict;
        var operationCalled = false;

        // act
        await result.IfFailureAsync(errors =>
        {
            operationCalled = true;
            return Task.CompletedTask;
        });

        // assert
        operationCalled.Should().BeTrue();
    }

    [TestMethod]
    public void IfSuccess_SkipsOperationCall_WhenResultFailure()
    {
        // arrange
        Result result = DefaultErrors.Conflict;
        var operationCalled = false;

        // act
        result.IfSuccess(() => { operationCalled = true; });

        // assert
        operationCalled.Should().BeFalse();
    }

    [TestMethod]
    public void IfSuccess_CallsOperation_WhenResultSuccess()
    {
        // arrange
        Result result = Result.Success();
        var operationCalled = false;

        // act
        result.IfSuccess(() => { operationCalled = true; });

        // assert
        operationCalled.Should().BeTrue();
    }

    [TestMethod]
    public async Task IfSuccessAsync_SkipsOperationCall_WhenResultFailure()
    {
        // arrange
        Result result = DefaultErrors.Conflict;
        var operationCalled = false;

        // act
        await result.IfSuccessAsync(() =>
        {
            operationCalled = true;
            return Task.CompletedTask;
        });

        // assert
        operationCalled.Should().BeFalse();
    }

    [TestMethod]
    public async Task IfSuccessAsync_CallsOperation_WhenResultSuccess()
    {
        // arrange
        Result result = Result.Success();
        var operationCalled = false;

        // act
        await result.IfSuccessAsync(() =>
        {
            operationCalled = true;
            return Task.CompletedTask;
        });


        // assert
        operationCalled.Should().BeTrue();
    }
}
