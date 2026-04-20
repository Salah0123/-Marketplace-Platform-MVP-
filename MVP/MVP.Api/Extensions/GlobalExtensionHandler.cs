using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVP.Application.Common.Extensions;

namespace MVP.Api.Extensions;

public sealed class GlobalExtensionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails = exception switch
        {
            ValidationException ex => new ValidationProblemDetails(ex.Errors.GroupBy(g=>g.PropertyName).ToDictionary(g=>g.Key, g => g.Select(e=>e.ErrorMessage).ToArray()))
            {
                Title = "Validation Failed",
                Status = StatusCodes.Status400BadRequest
            },
            NotFountExtension notFountExtension => new ProblemDetails
            {
                Title = "Not Found",
                Detail = notFountExtension.Message,
                Status = StatusCodes.Status404NotFound
            },
            _ => new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred. Please try again later.",
                Status = StatusCodes.Status500InternalServerError
            }
        };
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await _problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails,
        });
        return true;
    }
}
