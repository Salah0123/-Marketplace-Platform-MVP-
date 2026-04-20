using Microsoft.AspNetCore.Mvc;
using MVP.Domain.Common;

namespace MVP.Api.Abstractions;

public static class ResultExtensions
{
    public static ObjectResult ToProblem(this Result result, int statusCode)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert a successful result to a problem.");
        //var statusCode1 = result.Error.Code switch
        //{
        //    "NotFound" => 404,
        //    "Conflict" => 409,
        //    "BadRequest" => 400,
        //    _ => 500
        //};

        var problem = Results.Problem(statusCode: statusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;
        var errors = new Dictionary<string, List<string>>
            {
                { result.Error.Code, new List<string> { result.Error.Description } }
            };
        problemDetails!.Extensions = new Dictionary<string, object?>
            {
                {
                    "errors", errors // new[] { result.Error }
                }
            };
        return new ObjectResult(problemDetails);
    }
}
