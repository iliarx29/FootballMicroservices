namespace Auth.Application.Results;
public abstract class ResultBase
{
    public bool IsSuccess { get; set; }
    internal ErrorCode? Code { get; set; }
    internal string? CustomErrorMessage { get; set; } = string.Empty;
    public string ErrorMessage
    {
        get
        {
            if (IsSuccess)
            {
                throw new ResultException("There is not error message for successful result.");
            }

            return GetDescription();
        }
    }

    public ErrorCode? ErrorCode
    {
        get
        {
            if (IsSuccess)
            {
                throw new ResultException("There is not error code for successful result.");
            }

            return Code;
        }
    }

    protected ResultBase(bool isSuccess, Enum? code = null, string? customErrorMessage = null)
    {
        IsSuccess = isSuccess;
        Code = (ErrorCode?)code;
        CustomErrorMessage = customErrorMessage;
    }

    private string GetDescription()
    {
        if (!string.IsNullOrEmpty(CustomErrorMessage))
        {
            return Code?.GetDescription() + ". " + CustomErrorMessage;
        }

        return Code?.GetDescription();
    }
}
