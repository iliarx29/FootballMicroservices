namespace Matches.API.Common;

public class ApiResult<T>
{
    private ApiResult(int code, T result, IEnumerable<string> errors)
    {
        Code = code;
        Result = result;
        Errors = errors;
    }

    public int Code { get; }
    public T Result { get; }
    public IEnumerable<string> Errors { get; }

    public static ApiResult<T> Failure(int code, IEnumerable<string> errors)
    {
        return new ApiResult<T>(code, default, errors);
    }
}
