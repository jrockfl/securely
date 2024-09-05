using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Securely.Application.Helpers;
using Securely.Application.Results;
using Securely.Domain.Enities;
using Securely.Infrastructure.KeyVault;
using Securely.Infrastructure.Repositories;

namespace Securely.Application.Commands;

public class DeleteEncryptionKeyCommand : IDeleteEncryptionKeyCommand
{
    private readonly ICommandRepository _repository;
    private readonly IKeyVaultManager _keyVaultManager;
    private readonly ILogger<DeleteEncryptionKeyCommand> _logger;

    public DeleteEncryptionKeyCommand(ICommandRepository repository, IKeyVaultManager keyVaultManager, ILogger<DeleteEncryptionKeyCommand> logger)
    {
        _repository = repository;
        _keyVaultManager = keyVaultManager;
        _logger = logger;
    }

    public async Task<Result> DeleteEncryptionKeyAsync(Guid id)
    {
        Argument.AssertNotDefault(id, nameof(id));

        var keyVaultDeleted = false;
        var dbDeleted = false;

        Exception keyVaultException = null;
        Exception dbException = null;

        var failureReason = FailureReason.None;

        _logger.LogInformation($"Attempting to delete encryption key with id {id} from the database.");

        try
        {
            var encryptionKey = await _repository.Query<EncryptionKey>().FirstOrDefaultAsync(x => x.Id == id);

            if (encryptionKey != null)
            {
                _repository.Delete<EncryptionKey>(encryptionKey);
                await _repository.SaveChangesAsync();

                dbDeleted = true;
                _logger.LogInformation($"Encryption encryption key with id {id} has been deleted from the database.");
            }
        }
        catch (Exception ex)
        {
            dbException = ex;
            failureReason = failureReason == FailureReason.None ? FailureReason.Database : FailureReason.Partial;
            _logger.LogWarning(ex, $"Failed to delete encryption key with id {id} from database.");
        }

        _logger.LogInformation($"Retrieving encryption key with id {id} from the key vault.");

        try
        {
            await _keyVaultManager.DeleteKeyAsync(id.ToString());
            keyVaultDeleted = true;
            _logger.LogInformation($"Succesfully deleted encryption key with id {id} from the key vault.");
        }
        catch (Exception ex)
        {
            keyVaultException = ex;
            failureReason = FailureReason.Keyvault;
            _logger.LogWarning(ex, $"Failed to delete encryption key {id} from Azure Key Vault.");
        }

        if (keyVaultDeleted && dbDeleted)
        {
            return Result.Success();
        }

        return Result.Fail("Failed to delete the encryption key from all locations.", failureReason);
    }
}
