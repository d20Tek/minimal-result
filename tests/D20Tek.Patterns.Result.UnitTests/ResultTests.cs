//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result.UnitTests.Assertions;

namespace D20Tek.Patterns.Result.UnitTests;

[TestClass]
public sealed class ResultTests
{
    [TestMethod]
    public void ImplicitSuccess_ReturnsIsSuccess_True()
    {
        // arrange

        // act
        var result = Result.Success();

        // assert
        result.ShouldBeSuccess();
        result.Value.Should().Be("Ok");
        result.ValueOrDefault.Should().BeNull();
    }

    [TestMethod]
    public void ImplicitError_ReturnsIsFailure_True()
    {
        // arrange

        // act
        Result result = DefaultErrors.NotFound;

        // assert
        result.ShouldBeFailure(DefaultErrors.NotFound);
        result.Value.Should().Be("Failed");
        result.ValueOrDefault.Should().BeNull();
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
    public void ImplicitErrorsList_ReturnsIsFailure_True()
    {
        // arrange
        var errors = new List<Error> { DefaultErrors.NotFound, DefaultErrors.Conflict };

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
}
