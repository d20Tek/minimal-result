//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result.Extensions;
using FluentValidation.Results;

namespace D20Tek.Patterns.Result.UnitTests.Extensions;

[TestClass]
public sealed class FluentValidationExtensionsTests
{
    [TestMethod]
    public void ToErrors_WithValidationErrors_ReturnsConvertedList()
    {
        // arrange
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("foo", "error message 1"),
            new ValidationFailure("bar", "error message 2"),
            new ValidationFailure("yaz", "error message 3")
        });

        // act
        var errors = validationResult.ToErrors();

        // assert
        errors.Should().NotBeEmpty();
        errors.Should().HaveCount(3);
        errors.Should().Contain(x => x.Code == "foo");
    }

    [TestMethod]
    public void ToErrors_WithNoValidationErrors_ReturnsEmptyList()
    {
        // arrange
        var validationResult = new ValidationResult();

        // act
        var errors = validationResult.ToErrors();

        // assert
        errors.Should().BeEmpty();
    }

    [TestMethod]
    public void ToErrors_WithValidationErrorsAndCodes_ReturnsConvertedList()
    {
        // arrange
        var validationResult = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure { PropertyName = "foo", ErrorMessage = "error message 1", ErrorCode = "code1" },
            new ValidationFailure { PropertyName = "bar", ErrorMessage = "error message 2", ErrorCode = "code2" },
            new ValidationFailure { PropertyName = "yaz", ErrorMessage = "error message 3", ErrorCode = "code3" },
        });

        // act
        var errors = validationResult.ToErrors();

        // assert
        errors.Should().NotBeEmpty();
        errors.Should().HaveCount(3);
        errors.Should().NotContain(x => x.Code == "foo");
        errors.Should().Contain(x => x.Code == "code3");
    }
}
