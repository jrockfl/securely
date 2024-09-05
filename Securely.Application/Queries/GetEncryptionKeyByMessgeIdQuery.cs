using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Securely.Application.Helpers;
using Securely.Application.Models;
using Securely.Application.Results;
using Securely.Domain.Enities;
using Securely.Infrastructure.KeyVault;
using Securely.Infrastructure.Repositories;

namespace Securely.Application.Queries;
public class GetEncryptionKeyByMessgeIdQuery : IGetEncryptionKeyByMessgeIdQuery
{
    private readonly ILogger<GetEncryptionKeyByMessgeIdQuery> _logger;
    private readonly IKeyVaultManager _keyVaultManager;
    private readonly IQueryRepository _repository;

    public GetEncryptionKeyByMessgeIdQuery(ILogger<GetEncryptionKeyByMessgeIdQuery> logger, IKeyVaultManager keyVaultManager, IQueryRepository repository)
    {
        _logger = logger;
        _keyVaultManager = keyVaultManager;
        _repository = repository;
    }

    public async Task<Result<EncryptionKeyModel>> GetEncryptionKeyByMessageIdAsync(Guid messageId)
    {
        Argument.AssertNotDefault(messageId, nameof(messageId));

        _logger.LogInformation($"Retrieving encryption key with messageId {messageId} from the database.");

        var result = _repository.Query<SecureMessage>().FirstOrDefaultAsync(x => x.Id == messageId);

        if (result == null)
        {
            _logger.LogWarning($"The encryption key with  messageId {messageId} was not found.");
            return Result<EncryptionKeyModel>.Fail($"The encryption key with  messageId {messageId} was not found.", FailureReason.NotFound);
        }

        var encryptionKeyId = result.Result.EncryptionKeyId;
        string encryptionKey;

        _logger.LogInformation($"Successfully retrieved encryption key with messageId {messageId} from the database.");

        try
        {
            _logger.LogInformation($"Retrieving encryption key with messageId {messageId} from the key vault.");
            encryptionKey = await _keyVaultManager.GetKeyAsync(encryptionKeyId.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, ex.Message);
            return Result<EncryptionKeyModel>.Fail($"The encryption key with id {encryptionKeyId} was not found.", FailureReason.NotFound);
        }

        _logger.LogInformation($"Succesfully retrieved encryption key with id {encryptionKeyId} from the key vault.");

        var encryptionKeyModel = new EncryptionKeyModel
        {
            Id = encryptionKeyId,
            Key = encryptionKey
        };

        return Result<EncryptionKeyModel>.Success(encryptionKeyModel);
    }
}
