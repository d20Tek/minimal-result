//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Patterns.Result;

public static class DefaultErrors
{
    public static Error UnhandledExpection(string message) =>
        Error.Custom("General.UnhandledException", message, ErrorType.Unexpected);

    public static readonly Error Unexpected =
        Error.Unexpected("General.Unexpected", "An unexpected error has occurred.");

    public static readonly Error NotFound =
        Error.NotFound("General.NotFound", "Not found error has occurred.");

    public static readonly Error Conflict =
        Error.Conflict("General.Conflict", "A conflict error has occurred.");

    public static readonly Error Unauthorized =
        Error.Unauthorized("General.Unauthorized", "An anthentication/authorization error has occurred.");
}
