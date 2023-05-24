using Matches.Application.Result;

namespace Matches.API.Common;

public class ActionResultBody<T> : ActionResultBody
{
    public T Value { get; set; }

    public ActionResultBody(ErrorCode? code = null, string description = null, T value = default, string message = null)
        : base(code, description, message)
    {
        Value = value;
    }
}