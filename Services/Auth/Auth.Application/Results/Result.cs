namespace Auth.Application.Results;
public class Result : ResultBase
{
    public Result(bool isSuccess, Enum code, string? customErrorMessage = null)
        : base(isSuccess, code, customErrorMessage)
    { }

    public Result(bool isSuccess)
        : base(isSuccess)
    { }

    public static Result Success()
    {
        return new Result(true);
    }

    public static Result Error(Enum code, string? customeErrorMessage = null)
    {
        return new Result(false, code, customeErrorMessage);
    }
}
