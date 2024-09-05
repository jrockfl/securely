using Microsoft.AspNetCore.Mvc;

using Securely.Application.Events;
using Securely.Application.Mappers;
using Securely.Application.Requests;

namespace Securely.Api.Controllers;

[Route("api/cleanup")]
[ApiController]
public class CleanupController : BaseApiController
{
    private readonly ISendEventToServiceBusDispatcher _sendEventToServiceBusDispatcher;

    public CleanupController(ISendEventToServiceBusDispatcher eventDispatcher)
    {
        _sendEventToServiceBusDispatcher = eventDispatcher;
    }

    [HttpPost("")]
    public async Task<IActionResult> CleanupAsync([FromBody] CleanupRequest request)
    {
        if (request == null)
        {
            return BadRequest("The provided model was invalid or null.");
        }

        var model = EventMessageMapper.Map(request);

        await _sendEventToServiceBusDispatcher.ExecuteAsync(model);

        return CreateAcceptedResponse();
    }
}
