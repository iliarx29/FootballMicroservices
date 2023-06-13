using System.Net;

namespace Teams.API.Common;

public class ActionResultBody
{
    public HttpStatusCode Code { get; set; }
    public string? Message { get; set; }

    public ActionResultBody(HttpStatusCode code, string? message)
    {
        Code = code;
        Message = message;
    }
}
