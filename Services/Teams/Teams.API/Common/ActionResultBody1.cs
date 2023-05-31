using System.Net;

namespace Teams.API.Common;

public class ActionResultBody<T> : ActionResultBody
{
    public T Value { get; set; }

    public ActionResultBody(HttpStatusCode code, string message = null, T value = default)
        : base(code, message)
    {
        Value = value;
    }
}
