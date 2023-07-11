using D20Tek.Patterns.Result;

namespace Basic.WebApi;

internal static class ResultExtensions
{
    public static Result<TResult> MapResult<TValue, TResult>(
        this Result<TValue> result,
        Func<TValue, TResult> mapper) =>
        (result.IsSuccess) ? mapper(result.Value!) : result.Errors.ToArray();
}
