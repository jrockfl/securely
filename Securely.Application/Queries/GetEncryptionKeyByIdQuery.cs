using Microsoft.Extensions.Logging;

using Securely.Application.Helpers;
using Securely.Application.Models;
using Securely.Application.Results;
using Securely.Infrastructure.KeyVault;

namespace Securely.Application.Queries;
public class GetEncryptionKeyByIdQuery : IGetEncryptionKeyByIdQuery
{
    private readonly IKeyVaultManager _keyVaultManager;
    private readonly ILogger<GetEncryptionKeyByIdQuery> _logger;

    public GetEncryptionKeyByIdQuery(IKeyVaultManager keyVaultManager, ILogger<GetEncryptionKeyByIdQuery> logger)
    {
        _keyVaultManager = keyVaultManager;
        _logger = logger;
    }

    public async Task<Result<EncryptionKeyModel>> GetEncryptionKeyByIdAsync(Guid id)
    {
        Argument.AssertNotDefault(id, nameof(id));

        _logger.LogInformation($"Retrieving encryption key with id {id} from the key vault.");

        string encryptionKey;

        try
        {
            encryptionKey = await _keyVaultManager.GetKeyAsync(id.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, ex.Message);
            return Result<EncryptionKeyModel>.Fail($"The encryption key with id {id} was not found.", FailureReason.NotFound);
        }

        _logger.LogInformation($"Succesfully retrieved encryption key with id {id} from the key vault.");

        var encryptionKeyModel = new EncryptionKeyModel
        {
            Id = id,
            Key = encryptionKey
        };

        return Result<EncryptionKeyModel>.Success(encryptionKeyModel);
    }
}
