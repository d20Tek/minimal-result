//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Minimal.Result.UnitTests.Assertions;

internal static class ErrorAssertion
{
    public static void ShouldBe(
        this Error error,
        string expectedCode,
        string expectedMessage,
        int expectedType)
    {
        error.Should().NotBeNull();
        error.Code.Should().Be(expectedCode);
        error.Message.Should().Be(expectedMessage);
        error.Type.Should().Be(expectedType);
    }
}
