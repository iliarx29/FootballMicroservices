namespace Matches.Application.Results;

public class ResultBase
{
    public bool IsSuccess { get; set; }
    internal Enum Code { get; set; }
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

    public Enum ErrorCode
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

    protected ResultBase(bool isSuccess, Enum code = null, string customErrorMessage = null)
    {
        Code = code;
        IsSuccess = isSuccess;
        CustomErrorMessage = customErrorMessage;
    }
    private string GetDescription()
    {
        if (!string.IsNullOrEmpty(CustomErrorMessage))
        {
            return ErrorCode.GetDescription() + ". " + CustomErrorMessage;
        }

        return ErrorCode.GetDescription();
    }

}
