using Matches.Application.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace Matches.API.Common;

public class MatchesActionResult : IConvertToActionResult
{
    private readonly ActionResultBody _resultBody;

    internal HttpStatusCode HttpStatusCode { get; set; }
    internal ErrorCode? Code { get; set; }
    internal string Description { get; set; }
    internal string? Message { get; set; }

    public MatchesActionResult(HttpStatusCode httpStatusCode, ErrorCode? code = null, string? message = null)
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

    public MatchesActionResult Success()
    {
        return new MatchesActionResult(HttpStatusCode.OK, ErrorCode.OK);
    }

    public MatchesActionResult Fail(ErrorCode? code, string? message = null)
    {
        if (code is ErrorCode.NotFound)
        {
            return new MatchesActionResult(HttpStatusCode.NotFound, code, message);
        }

        return new MatchesActionResult(HttpStatusCode.BadRequest, code, message);
    }
}

public class MatchesActionResult<T> : MatchesActionResult, IConvertToActionResult
{
    private readonly ActionResultBody<T> _resultBody;

    public MatchesActionResult(HttpStatusCode httpStatusCode, ErrorCode? code = null, string? message = null, T value = default) : base(httpStatusCode, code)
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
    public MatchesActionResult<T> Success<T>(T value)
    {
        return new MatchesActionResult<T>(HttpStatusCode.OK, ErrorCode.OK, null, value);
    }

    public MatchesActionResult<T> Fail<T>(ErrorCode? code, string? message = null)
    {
        if (code is ErrorCode.NotFound)
        {
            return new MatchesActionResult<T>(HttpStatusCode.NotFound, code, message);
        }

        return new MatchesActionResult<T>(HttpStatusCode.BadRequest, code, message);
    }
}
