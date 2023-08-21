//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using FluentValidation;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.Patterns.Result.Extensions;

public class ResultValidationBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    private readonly IValidator<TRequest>? _validator;

    public ResultValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        return ConvertErrors(validationResult);

        [ExcludeFromCodeCoverage]
        static TResponse ConvertErrors(FluentValidation.Results.ValidationResult validationResult)
        {
            return (dynamic)validationResult.ToErrors();
        }
    }
}