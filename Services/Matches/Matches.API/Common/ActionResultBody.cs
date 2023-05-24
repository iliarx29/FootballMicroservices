using Matches.Application.Result;

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