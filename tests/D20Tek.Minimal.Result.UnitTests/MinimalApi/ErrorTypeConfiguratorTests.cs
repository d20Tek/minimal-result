//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result.AspNetCore;
using System.Net;

namespace D20Tek.Patterns.Result.UnitTests.MinimalApi;

[TestClass]
public class ErrorTypeConfiguratorTests
{
    [TestMethod]
    public void CreateDefaultConfigurator_ReturnsMappingEntryList()
    {
        // arrange

        // act
        var configurator = new ErrorTypeConfigurator();
        var result = configurator.Build();

        // assert
        result.Should().NotBeNull();
        result.Should().HaveCount(8);
        result.Should().Contain(
            new ConfigEntry(ErrorType.Unexpected, HttpStatusCode.InternalServerError));
        result.Should().Contain(
            new ConfigEntry(ErrorType.NotFound, HttpStatusCode.NotFound));
    }

    [TestMethod]
    public void Clear_ReturnsEmptyMappingEntryList()
    {
        // arrange
        var configurator = new ErrorTypeConfigurator();

        // act
        configurator.Clear();
        var result = configurator.Build();

        // assert
        result.Should().NotBeNull();
        result.Should().HaveCount(0);
        result.Should().NotContain(
            new ConfigEntry(ErrorType.Unexpected, HttpStatusCode.InternalServerError));
    }

    [TestMethod]
    public void For_WithNewErrorType_ReturnsItInMappingEntryList()
    {
        // arrange
        var configurator = new ErrorTypeConfigurator();

        // act
        configurator.For(100001, HttpStatusCode.MethodNotAllowed);
        var result = configurator.Build();

        // assert
        result.Should().NotBeNull();
        result.Should().HaveCount(9);
        result.Should().Contain(
            new ConfigEntry(100001, HttpStatusCode.MethodNotAllowed));
    }

    [TestMethod]
    public void For_WithExistingErrorType_UpdatesItInMappingEntryList()
    {
        // arrange
        var configurator = new ErrorTypeConfigurator();

        // act
        configurator.For(ErrorType.Invalid, HttpStatusCode.BadRequest);
        var result = configurator.Build();

        // assert
        var entry = new ConfigEntry(ErrorType.Invalid, HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result.Should().HaveCount(8);
        result.Should().Contain(entry);

        entry = new ConfigEntry(ErrorType.Invalid, HttpStatusCode.UnprocessableEntity);
        result.Should().NotContain(entry);
    }

    [TestMethod]
    public void Remove_WithExistingErrorType_RemovesItFromMappingEntryList()
    {
        // arrange
        var configurator = new ErrorTypeConfigurator();

        // act
        configurator.Remove(ErrorType.Forbidden);
        var result = configurator.Build();

        // assert
        result.Should().NotBeNull();
        result.Should().HaveCount(7);
        result.Should().NotContain(
            new ConfigEntry(ErrorType.Forbidden, HttpStatusCode.Forbidden));
    }

    [TestMethod]
    public void Remove_WithNonexistingErrorType_DoesnotChangeMappingEntryList()
    {
        // arrange
        var configurator = new ErrorTypeConfigurator();

        // act
        configurator.Remove(100001);
        var result = configurator.Build();

        // assert
        result.Should().NotBeNull();
        result.Should().HaveCount(8);
        result.Should().NotContain(x => x.ErrorType == 100001);
    }
}
