using CarBooks.Domain.Shared.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CarBooks.WebAPI;

/// <summary>
/// Translates the exceptions the application raises on purpose into RFC 7807 responses. Anything
/// else is left to the framework so it is logged and reported as a 500 without leaking details.
/// </summary>
internal sealed class CarBooksExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService problemDetailsService;
    private readonly ILogger<CarBooksExceptionHandler> logger;

    public CarBooksExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<CarBooksExceptionHandler> logger)
    {
        this.problemDetailsService = problemDetailsService;
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not CarBooksException carBooksException)
        {
            return false;
        }

        var statusCode = carBooksException switch
        {
            EntityNotFoundException => StatusCodes.Status404NotFound,
            DomainValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError,
        };

        logger.LogWarning(
            carBooksException,
            "Request {Path} failed with error code {ErrorCode}.",
            httpContext.Request.Path,
            carBooksException.ErrorCode);

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = carBooksException,
            ProblemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = statusCode == StatusCodes.Status404NotFound ? "Not found" : "Invalid request",
                Detail = carBooksException.Message,
                Extensions = { ["errorCode"] = carBooksException.ErrorCode },
            },
        });
    }
}
