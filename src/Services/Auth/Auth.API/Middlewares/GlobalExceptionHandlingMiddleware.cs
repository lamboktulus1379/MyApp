using System;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Auth.Core.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Enjoyer.API.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            Res res = new Res { ResponseCode = "500", ResponseMessage = "Internal server error" };
            string json = JsonSerializer.Serialize(res);
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(json);
        }
    }
}
