using FluentValidation;
using System.Net;
using System.Text.Json;
using Teams.Domain.Exceptions;

namespace Teams.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private Task HandleException(HttpContext context, Exception ex)
    {
        var code = ex switch
        {
            NotFoundException _ => HttpStatusCode.NotFound,
            ValidationException _ => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };

        var result = JsonSerializer.Serialize(new { error = ex.Message });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}
