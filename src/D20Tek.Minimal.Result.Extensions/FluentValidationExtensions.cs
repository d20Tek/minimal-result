﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using FluentValidation.Results;

namespace D20Tek.Minimal.Result.Extensions;

public static class FluentValidationExtensions
{
    public static List<Error> ToErrors(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .ConvertAll(validationFalure => Error.Validation(
                validationFalure.ErrorCode ?? validationFalure.PropertyName,
                validationFalure.ErrorMessage));

        return errors;
    }
}