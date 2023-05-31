namespace Matches.Application.Results;
public class Result : ResultBase
{
    public Result(bool isSuccess, Enum code, string customErrorMessage = null)
            : base(isSuccess, code, customErrorMessage) { }

    protected Result(bool isSuccess) : base(isSuccess) { }

    public static Result Success()
    {
        return new Result(true);
    }

    public static Result Error(Enum code, string customErrorMessage = null)
    {
        return new Result(false, code, customErrorMessage);
    }
}