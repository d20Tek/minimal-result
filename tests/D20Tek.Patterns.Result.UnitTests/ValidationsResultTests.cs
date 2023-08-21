//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Patterns.Result.UnitTests;

[TestClass]
public class ValidationsResultTests
{
    [TestMethod]
    public void AddOnFailure_WithConditionTrue_KeepsEmptyErrors()
    {
        // arrange
        var vResult = new ValidationsResult();

        // act
        vResult.AddOnFailure(() => true, CreateValidationError());

        // assert
        vResult.Errors.Should().BeEmpty();
        vResult.IsValid.Should().BeTrue();
    }

    [TestMethod]
    public void AddOnFailure_WithConditionFalse_AddsErrorToList()
    {
        // arrange
        var error = CreateValidationError();
        var vResult = new ValidationsResult();

        // act
        vResult.AddOnFailure(() => false, CreateValidationError());

        // assert
        vResult.Errors.Should().NotBeEmpty();
        vResult.Errors.Should().HaveCount(1);
        vResult.Errors.Should().Contain(error);
        vResult.IsValid.Should().BeFalse();
    }

    [TestMethod]
    public void AddValidationError_WithMultipleErrors_AddsToErrorList()
    {
        // arrange
        var vResult = new ValidationsResult();

        // act
        vResult.AddValidationError("test1", "message1");
        vResult.AddValidationError("test2", "message2");
        vResult.AddValidationError("test3", "message3");

        // assert
        vResult.Errors.Should().NotBeEmpty();
        vResult.Errors.Should().HaveCount(3);
        vResult.Errors.Should().Contain(x => x.Code == "test1");
        vResult.Errors.Should().Contain(x => x.Code == "test2");
        vResult.Errors.Should().Contain(x => x.Code == "test3");
    }

    [TestMethod]
    public void AddValidationError_WithError_AddsToErrorList()
    {
        // arrange
        var error = CreateValidationError();
        var vResult = new ValidationsResult();

        // act
        vResult.AddValidationError(error);

        // assert
        vResult.Errors.Should().NotBeEmpty();
        vResult.Errors.Should().HaveCount(1);
        vResult.Errors.Should().Contain(error);
    }

    [TestMethod]
    public void AddOperation_WithErrors_AddsToErrorList()
    {
        // arrange
        var error = CreateValidationError();
        var vResult = new ValidationsResult();
        vResult.AddValidationError(error);

        var error2 = CreateValidationError("foo", "bar");
        var v2 = new ValidationsResult();
        v2.AddValidationError(error2);

        // act
        vResult += v2;

        // assert
        vResult.Errors.Should().NotBeEmpty();
        vResult.Errors.Should().HaveCount(2);
        vResult.Errors.Should().Contain(error);
        vResult.Errors.Should().Contain(error2);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AddValidationError_WithUnexpectedError_ThrowsInvalidOperationException()
    {
        // arrange
        var vResult = new ValidationsResult();

        // act
        vResult.AddValidationError(DefaultErrors.Unexpected);

        // assert
    }

    [TestMethod]
    public void ToResult_WithErrors_ReturnsResult()
    {
        // arrange
        var error = CreateValidationError();
        var vResult = new ValidationsResult();

        vResult.AddValidationError(error);

        // act
        var result = vResult.ToResult<TestValue>();

        // assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(error);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ToResult_WithoutErrors_ThrowsInvalidOperationException()
    {
        // arrange
        var vResult = new ValidationsResult();
        vResult.AddOnFailure(() => true, CreateValidationError());

        // act
        _ = vResult.ToResult<TestValue>();

        // assert
    }

    [TestMethod]
    public void ToResult_WithoutErrorsButDefaultValue_ReturnValue()
    {
        // arrange
        var vResult = new ValidationsResult();
        vResult.AddOnFailure(() => true, CreateValidationError());

        var expected = new TestValue();

        // act
        var result = vResult.ToResult<TestValue>(expected);

        // assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    private Error CreateValidationError(string code = "test", string message = "error") =>
        Error.Validation(code, message);

    public class TestValue { };
}
