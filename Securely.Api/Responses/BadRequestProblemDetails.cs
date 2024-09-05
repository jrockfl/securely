using Microsoft.AspNetCore.Mvc;

namespace Securely.Api.Responses;

public sealed class BadRequestProblemDetails : ProblemDetails
{
    public BadRequestProblemDetails()
    {
        Status = StatusCodes.Status400BadRequest;
        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1";
    }
    public IReadOnlyCollection<string> Errors { get; set; }

    public string TracedId { get; set; }
}