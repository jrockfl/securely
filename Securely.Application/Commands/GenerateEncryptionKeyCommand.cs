using Microsoft.Extensions.Logging;

using Securely.Application.Mappers;
using Securely.Application.Models;
using Securely.Application.Results;
using Securely.Application.Security;
using Securely.Domain.Enities;
using Securely.Infrastructure.KeyVault;
using Securely.Infrastructure.Repositories;

namespace Securely.Application.Commands;
public class GenerateEncryptionKeyCommand : IGenerateEncryptionKeyCommand
{
    private readonly ICommandRepository _repository;
    private readonly IEncryptionKeyGenerator _encryptionKeyGenerator;
    private readonly IKeyVaultManager _keyVaultManager;
    private readonly ILogger<GenerateEncryptionKeyCommand> _logger;

    public GenerateEncryptionKeyCommand(ICommandRepository repository, IEncryptionKeyGenerator encryptionKeyGenerator, IKeyVaultManager keyVaultManager, ILogger<GenerateEncryptionKeyCommand> logger)
    {
        _repository = repository;
        _encryptionKeyGenerator = encryptionKeyGenerator;
        _keyVaultManager = keyVaultManager;
        _logger = logger;
    }

    public async Task<Result<EncryptionKeyModel>> GenerateEncryptionKeyAsync()
    {
        var encryptionKey = new EncryptionKey();
        var key = GenerateAesKey();
        _logger.LogInformation("Generated Aes security key successfully.");

        _logger.LogInformation("Saving Aes key to key vault.");
        await SaveKeyToKeyVaultAsync(encryptionKey.Id.ToString(), key);
        _logger.LogInformation("Saved Aes security key to key vault successfully.");

        _logger.LogInformation("Saving Aes key id to database.");
        _repository.Add(encryptionKey);
        await _repository.SaveChangesAsync();
        _logger.LogInformation("Saved Aes key id to database successfully.");

        var result = EncryptionKeyMapper.Map(encryptionKey, key);

        return Result<EncryptionKeyModel>.Success(result);
    }

    private string GenerateAesKey()
    {
        var key = _encryptionKeyGenerator.GenerateAesKey();
        return key;
    }

    private async Task SaveKeyToKeyVaultAsync(string id, string key)
    {
        await _keyVaultManager.SaveKeyAsync(id, key);
    }
}
