using Matches.Application.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace Matches.API.Common;

public class CustomActionResult : IConvertToActionResult
{
    private readonly ActionResultBody _resultBody;

    internal HttpStatusCode HttpStatusCode { get; set; }
    internal ErrorCode? Code { get; set; }
    internal string Description { get; set; }
    internal string? Message { get; set; }

    public CustomActionResult(HttpStatusCode httpStatusCode, ErrorCode? code = null, string? message = null)
    {
        Code = code;
        Description = code.GetDescription();
        HttpStatusCode = httpStatusCode;
        _resultBody = new ActionResultBody(Code);
        Message = message;
    }

    public IActionResult Convert()
    {
        ObjectResult objectResult = new ObjectResult(_resultBody)
        {
            StatusCode = (int)HttpStatusCode
        };

        return objectResult;
    }

    public CustomActionResult Success()
    {
        return new CustomActionResult(HttpStatusCode.OK, ErrorCode.OK);
    }

    public CustomActionResult Fail(ErrorCode? code, string? message = null)
    {
        if (code is ErrorCode.NotFound)
        {
            return new CustomActionResult(HttpStatusCode.NotFound, code, message);
        }

        return new CustomActionResult(HttpStatusCode.BadRequest, code, message);
    }
}