using Bss.Infrastructure.Errors.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Bss.Infrastructure.Errors;

internal class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            var cancellationToken = httpContext?.RequestAborted ?? CancellationToken.None;

            switch (ex)
            {
                case ValidationException validationException:
                    await HandleValidationException(httpContext!, validationException, cancellationToken);
                    break;
                case OperationException operationException:
                    await HandleOperationException(httpContext!, operationException, cancellationToken);
                    break;
                case NotFoundException notFoundException:
                    await HandleNotFoundException(httpContext!, notFoundException, cancellationToken);
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    await HandleUnauthorizedAccessException(httpContext!, unauthorizedAccessException, cancellationToken);
                    break;
                default:
                    await HandleUnknownExceptionAsync(httpContext!, ex, cancellationToken);
                    break;
            }
        }
    }

    private static async Task HandleValidationException(HttpContext httpContext, ValidationException exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(exception.ValidationResult, cancellationToken);
    }

    private static async Task HandleOperationException(HttpContext httpContext, OperationException exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(OperationErrorResult.FromException(exception), cancellationToken);
    }

    private static async Task HandleNotFoundException(HttpContext httpContext, NotFoundException exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails()
            {
                Status = StatusCodes.Status404NotFound,
                Title = "The specified resource was not found.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Detail = exception.Message
            },
            cancellationToken);
    }

    private static async Task HandleUnauthorizedAccessException(HttpContext httpContext, UnauthorizedAccessException exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                Detail = exception.Message
            }, 
            cancellationToken: cancellationToken);
    }

    private static async Task HandleUnknownExceptionAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            },
            cancellationToken);
    }
}
