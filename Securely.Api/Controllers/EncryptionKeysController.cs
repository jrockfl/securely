using Microsoft.AspNetCore.Mvc;

using Securely.Application.Commands;
using Securely.Application.Queries;

namespace Securely.Api.Controllers;
[Route("api/encryptionKeys")]
[ApiController]
public class EncryptionKeysController : BaseApiController
{
    private readonly IGenerateEncryptionKeyCommand _generateEncryptionKeyCommand;
    private readonly IGetEncryptionKeyByIdQuery _getEncryptionKeyByIdQuery;
    private readonly IGetEncryptionKeyByMessgeIdQuery _getEncryptionKeyByMessgeId;
    private readonly IDeleteEncryptionKeyCommand _deleteEncryptionKeyCommand;

    public EncryptionKeysController(IGenerateEncryptionKeyCommand generateEncryptionKeyCommand, IGetEncryptionKeyByIdQuery getEncryptionKeyByIdQuery, IGetEncryptionKeyByMessgeIdQuery getEncryptionKeyByMessgeId, IDeleteEncryptionKeyCommand deleteEncryptionKeyCommand)
    {
        _generateEncryptionKeyCommand = generateEncryptionKeyCommand;
        _getEncryptionKeyByIdQuery = getEncryptionKeyByIdQuery;
        _getEncryptionKeyByMessgeId = getEncryptionKeyByMessgeId;
        _deleteEncryptionKeyCommand = deleteEncryptionKeyCommand;
    }

    [HttpPost("")]
    public async Task<IActionResult> GenerateEncryptionKeyAsync()
    {
        var result = await _generateEncryptionKeyCommand.GenerateEncryptionKeyAsync();

        if (result.IsSuccess)
        {
            return CreateSuccessResponse(result.Value);
        }

        return HandleFailure(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEncryptionKeyAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("The provided id was invalid.");
        }

        var result = await _getEncryptionKeyByIdQuery.GetEncryptionKeyByIdAsync(id);

        return result.IsSuccess ? CreateSuccessResponse(result.Value) : CreateNotFoundResponse(result.Error);
    }

    [HttpGet("~/api/secureMessages/{id:guid}/encryptionKey")]
    public async Task<IActionResult> GetEncryptionKeyByMessageIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("The provided id was invalid.");
        }

        var result = await _getEncryptionKeyByMessgeId.GetEncryptionKeyByMessageIdAsync(id);

        return result.IsSuccess ? CreateSuccessResponse(result.Value) : CreateNotFoundResponse(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEncryptionKeyAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("The provided id was invalid.");
        }

        var result = await _deleteEncryptionKeyCommand.DeleteEncryptionKeyAsync(id);

        if (result.IsSuccess)
        {
            return CreateNoContentResponse();
        }

        return HandleFailure(result);
    }
}
