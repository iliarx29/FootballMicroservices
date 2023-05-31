namespace Matches.Application.Results;
public class Result<T> : ResultBase
{
    private readonly T _value;
    public T Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new ResultException("You attempted to access the Value property for a failed result. A failed result has no Value YET.");
            }

            return _value;
        }
    }

    protected Result(bool isSuccess, Enum code, string customErrorMessage = null)
            : base(isSuccess, code, customErrorMessage)
    {
        _value = default;
    }
    protected Result(bool isSuccess, T result) : base(isSuccess)
    {
        _value = result;
    }

    public static Result<T> Success(T result)
    {
        return new Result<T>(true, result);
    }

    public static Result<T> Error(Enum code, string customErrorMessage)
    {
        return new Result<T>(false, code, customErrorMessage);
    }
}