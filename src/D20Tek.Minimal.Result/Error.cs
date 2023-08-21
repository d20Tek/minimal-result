//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace D20Tek.Patterns.Result;

public readonly struct Error
{
    public int Type { get; }

    public string Code { get; }

    public string Message { get; }

    private Error(string code, string message, int errorType)
    {
        Type = errorType;
        Code = code;
        Message = message;
    }

    public static Error Unexpected(string code, string message) =>
        new Error(code, message, ErrorType.Unexpected);

    public static Error Failure(string code, string message) =>
        new Error(code, message, ErrorType.Failure);

    public static Error Validation(string code, string message) =>
        new Error(code, message, ErrorType.Validation);

    public static Error NotFound(string code, string message) =>
        new Error(code, message, ErrorType.NotFound);

    public static Error Conflict(string code, string message) =>
        new Error(code, message, ErrorType.Conflict);

    public static Error Unauthorized(string code, string message) =>
        new Error(code, message, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string message) =>
        new Error(code, message, ErrorType.Forbidden);

    public static Error Invalid(string code, string message) =>
        new Error(code, message, ErrorType.Invalid);

    public static Error Custom(string code, string message, int errorType) =>
        new Error(code, message, errorType);

    public override string ToString() => $"Error ({Code} [{Type}]): {Message}";
}