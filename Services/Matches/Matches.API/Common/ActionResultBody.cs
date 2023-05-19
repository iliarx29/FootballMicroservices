using Matches.Application.Results;

namespace Matches.API.Common;

public class ActionResultBody
{
    public int Code { get; set; }
    public string Description { get; set; }
    public string Message { get; set; }

    public ActionResultBody(ErrorCode? code = null, string description = null, string message = null)
    {
        Code = (int)code;
        Description = description;
        Message = message;
    }
}

public class ActionResultBody<T> : ActionResultBody
{
    public T Value { get; set; }

    public ActionResultBody(ErrorCode? code = null, string description = null, T value = default, string message = null)
        : base(code, description, message)
    {
        Value = value;
    }
}
