using System.Diagnostics;
using System.Net;

using Microsoft.AspNetCore.Mvc;

using Securely.Api.Responses;
using Securely.Application.Results;

namespace Securely.Api.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    protected IActionResult HandleFailure<T>(Result<T> result)
    {
        switch (result.FailureReason)
        {
            case FailureReason.BadRequest:
                return BadRequest(CreateProblemDetails("Bad Request", result.Errors));

            case FailureReason.NotFound:
                return CreateNotFoundResponse(result.Error);

            case FailureReason.Gone:
            case FailureReason.Expired:
                return CreateGoneResponse(result.Error);

            default:
                return CreateErrorResponse(result.Error);
        }
    }

    protected IActionResult HandleFailure(Result result)
    {
        switch (result.FailureReason)
        {
            case FailureReason.BadRequest:
                return BadRequest(CreateProblemDetails("Bad Request", result.Errors));

            case FailureReason.NotFound:
                return CreateNotFoundResponse(result.Error);

            case FailureReason.Gone:
                return CreateGoneResponse(result.Error);

            default:
                return CreateErrorResponse(result.Error);
        }
    }

    private static ProblemDetails CreateProblemDetails(string title, IReadOnlyCollection<string> errors)
    {
        return new BadRequestProblemDetails { Title = title, Errors = errors, TracedId = Activity.Current?.Id };
    }

    protected IActionResult CreateSuccessResponse<T>(T data)
    {
        return Ok(new ApiResponse<T>(data));
    }

    protected IActionResult CreateNoContentResponse()
    {
        return NoContent();
    }

    protected IActionResult CreateNotFoundResponse(string message)
    {
        return NotFound(message);
    }

    protected IActionResult CreateGoneResponse(string message)
    {
        return this.StatusCode((int)HttpStatusCode.Gone);
    }

    protected IActionResult CreateErrorResponse(string message)
    {
        return this.StatusCode((int)HttpStatusCode.InternalServerError);
    }

    protected IActionResult CreateAcceptedResponse()
    {
        return Accepted();
    }
}
