using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace Teams.API.Common;

public class CustomActionResult : IConvertToActionResult
{
    private ActionResultBody _body;

    public HttpStatusCode StatusCode { get; set; }
    internal string? Message { get; set; }

    public CustomActionResult(HttpStatusCode httpStatusCode, string? message = null)
    {
        StatusCode = httpStatusCode;
        Message = message;
        _body = new ActionResultBody(StatusCode, Message);
    }

    public IActionResult Convert()
    {
        ObjectResult objectResult = new ObjectResult(_body)
        {
            StatusCode = (int)StatusCode
        };
        return objectResult;
    }
}