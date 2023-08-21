//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.Patterns.Result.Extensions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace D20Tek.Patterns.Result.UnitTests.Extensions;

[TestClass]
public sealed class ResultValidationBehaviorTests
{
    private readonly CancellationToken _token = new CancellationToken();
    private readonly TestResponse _validResponse = new TestResponse(42, "test");

    [TestMethod]
    public async Task Handle_CalledWithNoValidator_ReturnsNextDelegate()
    {
        // arrange
        var behavior = new ResultValidationBehavior<TestRequest, Result<TestResponse>>(null);
        var request = new TestRequest(42);

        // act
        var result = await behavior.Handle(request, () => HandleNext(), _token);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(_validResponse);
    }

    [TestMethod]
    public async Task Handle_CalledWithNoValidationErrors_ReturnsNextDelegate()
    {
        // arrange
        var validator = new EmptyValidator();
        var behavior = new ResultValidationBehavior<TestRequest, Result<TestResponse>>(validator);
        var request = new TestRequest(42);

        // act
        var result = await behavior.Handle(request, () => HandleNext(), _token);

        // assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(_validResponse);
    }

    [TestMethod]
    public async Task Handle_CalledWithValidationErrors_ReturnsNextDelegate()
    {
        // arrange
        var validator = new ErrorsValidator();
        var behavior = new ResultValidationBehavior<TestRequest, Result<TestResponse>>(validator);
        var request = new TestRequest(42);

        // act
        var result = await behavior.Handle(
            request,
            [ExcludeFromCodeCoverage] () => HandleNext(),
            _token);

        // assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.ValueOrDefault.Should().BeNull();
        result.Errors.Should().HaveCount(1);
    }

    private Task<Result<TestResponse>> HandleNext() =>
        Task.FromResult(Result<TestResponse>.Success(_validResponse));

    private sealed class EmptyValidator : AbstractValidator<TestRequest>
    {
    }

    private sealed class ErrorsValidator : AbstractValidator<TestRequest>
    {
        public ErrorsValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(100);
        }
    }
}
