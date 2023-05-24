using Matches.Application.Result;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Matches.API.Common;

public class CustomActionResult<T> : CustomActionResult, IConvertToActionResult
{
    private readonly ActionResultBody<T> _resultBody;

    public CustomActionResult(HttpStatusCode httpStatusCode, ErrorCode? code = null, string? message = null, T value = default) : base(httpStatusCode, code)
    {
        _resultBody = new ActionResultBody<T>(code, Description, value, message);
    }

    public IActionResult Convert()
    {
        ObjectResult objectResult = new ObjectResult(_resultBody)
        {
            StatusCode = (int)HttpStatusCode
        };

        return objectResult;
    }
    public CustomActionResult<T> Success<T>(T value)
    {
        return new CustomActionResult<T>(HttpStatusCode.OK, ErrorCode.OK, null, value);
    }

    public CustomActionResult<T> Fail<T>(ErrorCode? code, string? message = null)
    {
        if (code is ErrorCode.NotFound)
        {
            return new CustomActionResult<T>(HttpStatusCode.NotFound, code, message);
        }

        return new CustomActionResult<T>(HttpStatusCode.BadRequest, code, message);
    }
}
