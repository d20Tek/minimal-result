//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Patterns.Result.UnitTests.Assertions;

internal static class ResultAssertion
{
    public static void ShouldBeSuccess(this Result result)
    {
        result.Should().NotBeNull();
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    public static void ShouldBeFailure(
        this Result result, params Error[] expectedErrors)
    {
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.ValueOrDefault.Should().BeNull();
        result.Errors.Should().NotBeNull();

        foreach (var error in expectedErrors)
        {
            result.Errors.Should().Contain(error);
        }
    }
}
