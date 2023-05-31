namespace Auth.Application.Results;
public class Result<T> : ResultBase
{
    private readonly T? _value;
    public T? Value
    {
        get
        {
            if (!IsSuccess)
                throw new ResultException("You attempted to access the Value property for a failed result. A failed result has no Value YET." +
                    "Code Description: " + ErrorCode?.GetDescription());

            return _value;
        }
    }

    protected Result(bool isSuccess, Enum code, string? customErrorMessage = null)
        : base(isSuccess, code, customErrorMessage)
    {
        _value = default;
    }

    protected Result(bool isSuccess, T value)
        : base(isSuccess)
    {
        _value = value;
    }

    private Result(Result result)
        : base(result.IsSuccess)
    {
        if (!result.IsSuccess)
        {
            Code = result.ErrorCode;
        }
        else
        {
            Code = null;
        }

        _value = default;
        CustomErrorMessage = null;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value);
    }

    public static Result<T> Error(Enum code, string? customErrorMessage = null)
    {
        return new Result<T>(false, code, customErrorMessage);
    }

    public static implicit operator Result<T>(Result result)
    {
        return new Result<T>(result);
    }

}
