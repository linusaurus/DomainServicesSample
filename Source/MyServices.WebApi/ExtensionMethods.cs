namespace MyServices.WebApi;

using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public static class ExtensionMethods
{
    public static Task ToHttpResponse(this Exception ex, HttpContext context)
    {
        HttpStatusCode code;
        if (ex is KeyNotFoundException || ex is ArgumentOutOfRangeException)
        {
            code = HttpStatusCode.NotFound;
        }
        else if (ex is ArgumentException)
        {
            code = HttpStatusCode.BadRequest;
        }
        else if (ex is NotImplementedException || ex is NotSupportedException)
        {
            code = HttpStatusCode.NotImplemented;
        }
        else
        {
            code = HttpStatusCode.InternalServerError;
        }

        var result = JsonSerializer.Serialize(new { error = ex.ToString() });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }

    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
