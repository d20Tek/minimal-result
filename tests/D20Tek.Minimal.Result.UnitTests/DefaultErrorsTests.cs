//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result.UnitTests.Assertions;

namespace D20Tek.Minimal.Result.UnitTests;

[TestClass]
public sealed class DefaultErrorsTests
{
    [TestMethod]
    public void FactoryMethod_ShouldCreate_UnhandledExceptionError()
    {
        PerformErrorTest(
            "General.Unexpected",
            "An unexpected error has occurred.",
            ErrorType.Unexpected,
            DefaultErrors.Unexpected);
    }

    [TestMethod]
    public void FactoryMethod_ShouldCreate_NotFoundError()
    {
        PerformErrorTest(
            "General.NotFound",
            "Not found error has occurred.",
            ErrorType.NotFound,
            DefaultErrors.NotFound);
    }

    [TestMethod]
    public void FactoryMethod_ShouldCreate_ConflictError()
    {
        PerformErrorTest(
            "General.Conflict",
            "A conflict error has occurred.",
            ErrorType.Conflict,
            DefaultErrors.Conflict);
    }

    [TestMethod]
    public void FactoryMethod_ShouldCreate_UnauthorizedError()
    {
        PerformErrorTest(
            "General.Unauthorized",
            "An anthentication/authorization error has occurred.",
            ErrorType.Unauthorized,
            DefaultErrors.Unauthorized);
    }

    [TestMethod]
    public void FactoryMethod_ShouldCreate_UnhandledExpectionError()
    {
        PerformErrorTest(
            "General.Exception",
            "An unhandled exception has occurred.",
            ErrorType.Unexpected,
            DefaultErrors.UnhandledExpection);
    }

    private void PerformErrorTest(
        string expectedCode,
        string expectedMessage,
        int exptectedType,
        Error error)
    {
        // arrange

        // act

        // assert
        error.ShouldBe(expectedCode, expectedMessage, exptectedType);
    }

    private void PerformErrorTest(
        string expectedCode,
        string message,
        int exptectedType,
        Func<string, Error> operation)
    {
        // arrange

        // act
        var error = operation(message);

        // assert
        error.ShouldBe(expectedCode, message, exptectedType);
    }
}
