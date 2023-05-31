﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace Teams.API.Common;

public class CustomActionResult : IConvertToActionResult
{
    private ActionResultBody _body;

    internal HttpStatusCode HttpStatusCode { get; set; }
    internal string? Message { get; set; }

    public CustomActionResult(HttpStatusCode httpStatusCode, string? message = null)
    {
        HttpStatusCode = httpStatusCode;
        Message = message;
        _body = new ActionResultBody(HttpStatusCode, Message);
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