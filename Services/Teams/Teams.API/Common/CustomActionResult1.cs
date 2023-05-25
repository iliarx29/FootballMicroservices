using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using Teams.Domain.Results;

namespace Teams.API.Common;

public class CustomActionResult<T> : IConvertToActionResult
{
    public readonly ActionResultBody<T> ResultBody;
    public readonly HttpStatusCode StatusCode;

    public readonly CustomActionResult CustomResult;

    public CustomActionResult(HttpStatusCode httpStatusCode, T value = default)
    {
        StatusCode = httpStatusCode;
        string description = httpStatusCode.GetDescription();
        ResultBody = new ActionResultBody<T>(StatusCode, null, value);
    }

    public CustomActionResult(HttpStatusCode statusCode, string message, T value = default)
    {
        StatusCode = statusCode;
        ResultBody = new ActionResultBody<T>(statusCode, message, value);
    }

    public CustomActionResult(CustomActionResult result)
    {
        CustomResult = result;
    }

    public IActionResult Convert()
    {
        ObjectResult objectResult = new ObjectResult(ResultBody)
        {
            StatusCode = (int)StatusCode
        };

        IActionResult result = (IActionResult)CustomResult;

        return result ?? objectResult;
    }

    public static implicit operator CustomActionResult<T>(CustomActionResult result)
    {
        return new CustomActionResult<T>(result);
    }

    public static implicit operator ObjectResult(CustomActionResult<T> result)
    {
        return new ObjectResult(result.ResultBody)
        {
            StatusCode = (int)result.StatusCode
        };
    }
}

