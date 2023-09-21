//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Text.Json;
using System.Text.Json.Serialization;

namespace D20Tek.Minimal.Result.Client;

public class ProblemDetails
{
    public string? Type { get; set; }

    public string? Title { get; set; }

    public int? Status { get; set; } = 500;

    public string? Detail { get; set; }

    public string? Instance { get; set; }

    public Dictionary<string, object?>? Errors { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object?> Extensions { get; set; } =
        new Dictionary<string, object?>(StringComparer.Ordinal);

    public Result<T> ToResult<T>() where T : class
    {
        List<Error> errors = new();

        if (Errors is not null)
        {
            errors = Errors.Select(x =>
            {
                string message = GetMessageElement(x);
                return Error.Custom(x.Key, message, Status ?? 500);
            }).ToList();
        }

        return errors;
    }

    private string GetMessageElement(KeyValuePair<string, object?> elementObject)
    {
        var element = (JsonElement)elementObject.Value!;
        string message;

        if (element.ValueKind == JsonValueKind.Array)
        {
            var messages = element.Deserialize<string[]>()!;
            message = messages.FirstOrDefault()?.ToString() ?? "Unexpected error occurred.";
        }
        else
        {
            message = element.ToString();
        }

        return message;
    }
}
