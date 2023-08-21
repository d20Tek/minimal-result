//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Minimal.Result.AspNetCore;
using System.Net;

namespace D20Tek.Minimal.Result.UnitTests.MinimalApi;

[TestClass]
public class ErrorTypeMapperTests
{
    [TestMethod]
    public void Instance_ReturnsDefaultConfiguration()
    {
        // arrange

        // act
        var mapper = ErrorTypeMapper.Instance;

        // assert
        mapper.Should().NotBeNull();
        mapper.Contains(ErrorType.Unauthorized).Should().BeTrue();
        mapper.Convert(ErrorType.Unauthorized).Should().Be(HttpStatusCode.Unauthorized);
        mapper.Contains(ErrorType.Validation).Should().BeTrue();
        mapper.Convert(ErrorType.Validation).Should().Be(HttpStatusCode.BadRequest);
        mapper.Contains(ErrorType.Invalid).Should().BeTrue();
        mapper.Convert(ErrorType.Invalid).Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [TestMethod]
    public void Configure_ShouldUpdateDefaultConfiguration()
    {
        // arrange
        var mapper = ErrorTypeMapper.Instance;

        // act
        mapper.Configure(config =>
            config.For(ErrorType.Invalid, HttpStatusCode.BadRequest)
                  .Remove(ErrorType.Forbidden));

        // assert
        mapper.Should().NotBeNull();
        mapper.Contains(ErrorType.Invalid).Should().BeTrue();
        mapper.Convert(ErrorType.Invalid).Should().Be(HttpStatusCode.BadRequest);
        mapper.Contains(ErrorType.Forbidden).Should().BeFalse();

        // cleanup
        mapper.Configure();
    }

    [TestMethod]
    public void ClearedMapper_ShouldAlwaysReturn_InternalServiceError()
    {
        // arrange
        var mapper = ErrorTypeMapper.Instance;

        // act
        mapper.Configure(config => config.Clear());

        // assert
        mapper.Should().NotBeNull();
        mapper.Contains(ErrorType.Invalid).Should().BeFalse();
        mapper.Convert(ErrorType.Invalid).Should().Be(HttpStatusCode.InternalServerError);
        mapper.Contains(ErrorType.Unexpected).Should().BeFalse();
        mapper.Convert(ErrorType.Unexpected).Should().Be(HttpStatusCode.InternalServerError);

        // cleanup
        mapper.Configure();
    }

    [TestMethod]
    public void RemovedErrorType_ShouldNotBeReturned()
    {
        // arrange
        int errorType = 10000001;
        var mapper = ErrorTypeMapper.Instance;
        mapper.For(errorType, HttpStatusCode.NotFound);

        // act
        mapper.Remove(errorType);

        // assert
        mapper.Should().NotBeNull();
        mapper.Contains(errorType).Should().BeFalse();
        mapper.Convert(errorType).Should().Be(HttpStatusCode.InternalServerError);
    }
}
