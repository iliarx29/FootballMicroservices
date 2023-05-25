using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using Teams.Domain.Results;

namespace Teams.API;

public class CustomActionResult : IConvertToActionResult
{
    private ActionResultBody _body;

    internal HttpStatusCode HttpStatusCode { get; set; }
    internal string Description { get; set; }
    internal Enum Code1 { get; set; }

    public CustomActionResult(HttpStatusCode httpStatusCode, Enum code)
    {
        HttpStatusCode = httpStatusCode;
        Code1 = code;
        Description = code.GetDescription();
        _body = new ActionResultBody(Code1, Description);
    }

    public IActionResult Convert()
    {
        ObjectResult objectResult = new ObjectResult(_body)
        {
            StatusCode = (int)HttpStatusCode
        };
        return objectResult;
    }
}