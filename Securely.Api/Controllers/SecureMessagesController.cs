using Microsoft.AspNetCore.Mvc;

using Securely.Application.Commands;
using Securely.Application.Models;
using Securely.Application.Queries;

namespace Securely.Api.Controllers;
[Route("api/secureMessages")]
[ApiController]
public class SecureMessagesController : BaseApiController
{
    private readonly ICreateSecureMessageCommand _createSecureMessageCommand;
    private readonly IGetSecureMessageByIdQuery _getSecureMessageByIdQuery;
    private readonly IUpdateSecureMessageCommand _clearSecureMessageCommand;
    private readonly IDeleteSecureMessageCommand _deleteSecureMessageCommand;

    public SecureMessagesController(ICreateSecureMessageCommand createSecureMessageCommand, IGetSecureMessageByIdQuery getSecureMessageByIdQuery, IUpdateSecureMessageCommand clearSecureMessageCommand, IDeleteSecureMessageCommand deleteSecureMessageCommand)
    {
        _createSecureMessageCommand = createSecureMessageCommand;
        _getSecureMessageByIdQuery = getSecureMessageByIdQuery;
        _clearSecureMessageCommand = clearSecureMessageCommand;
        _deleteSecureMessageCommand = deleteSecureMessageCommand;
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateSecureMessageAsync([FromBody] CreateSecureMessageModel request)
    {
        if (request == null)
        {
            return BadRequest("The provided model was invalid or null.");
        }

        var result = await _createSecureMessageCommand.CreateSecureMessageAsync(request);

        if (result.IsSuccess)
        {
            return CreateSuccessResponse(result.Value);
        }

        return HandleFailure(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSecureMessageAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("The provided id was invalid.");
        }

        var result = await _getSecureMessageByIdQuery.GetSecureMessageByIdAsync(id);

        if (result.IsSuccess)
        {
            return CreateSuccessResponse(result.Value);
        }

        return HandleFailure(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateSecureMessageAsync([FromBody] UpdateSecureMessageModel request)
    {
        if (request == null)
        {
            return BadRequest("The provided model was invalid or null.");
        }

        var result = await _clearSecureMessageCommand.UpdateSecureMessageAsync(request);

        if (result.IsSuccess)
        {
            return CreateSuccessResponse(result.Value);
        }

        return HandleFailure(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSecureMessageAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("The provided id was invalid.");
        }

        var result = await _deleteSecureMessageCommand.DeleteSecureMessageAsync(id);

        if (result.IsSuccess)
        {
            return CreateNoContentResponse();
        }

        return HandleFailure(result);
    }
}
