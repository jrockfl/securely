using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Securely.Application.Helpers;
using Securely.Application.Models;
using Securely.Application.Results;
using Securely.Domain.Enities;
using Securely.Domain.Enums;
using Securely.Infrastructure.Repositories;

namespace Securely.Application.Commands;

public class DeleteSecureMesssageAndEncryptionKeyCommand : IDeleteSecureMesssageAndEncryptionKeyCommand
{
    private readonly IDeleteEncryptionKeyCommand _deleteEncryptionKeyCommand;
    private readonly ILogger<DeleteSecureMesssageAndEncryptionKeyCommand> _logger;
    private readonly ICommandRepository _repository;
    private readonly ICreateSecureMessageHistoryCommand _historyCommand;

    public DeleteSecureMesssageAndEncryptionKeyCommand(IDeleteEncryptionKeyCommand deleteEncryptionKeyCommand, ILogger<DeleteSecureMesssageAndEncryptionKeyCommand> logger, ICommandRepository repository, ICreateSecureMessageHistoryCommand historyCommand)
    {
        _deleteEncryptionKeyCommand = deleteEncryptionKeyCommand;
        _logger = logger;
        _repository = repository;
        _historyCommand = historyCommand;
    }

    public async Task<Result> ExecuteAsync(Guid secureMessageId, Guid encryptionKeyId)
    {
        Argument.AssertNotDefault(secureMessageId, nameof(secureMessageId));
        Argument.AssertNotDefault(encryptionKeyId, nameof(encryptionKeyId));

        _logger.LogInformation("Starting deletion of SecureMessage and EncryptionKey. SecureMessageId: {SecureMessageId}, EncryptionKeyId: {EncryptionKeyId}", secureMessageId, encryptionKeyId);

        var secureMessage = await _repository.Query<SecureMessage>().FirstOrDefaultAsync(x => x.Id == secureMessageId);

        if (secureMessage == null)
        {
            _logger.LogError($"The secure message with id {secureMessageId} does not exist in the database.");
            return Result.Fail($"The secure message with id {secureMessageId} does not exist.", FailureReason.NotFound);
        }

        var deleteEncryptionKey = await _deleteEncryptionKeyCommand.DeleteEncryptionKeyAsync(encryptionKeyId);

        if (!deleteEncryptionKey.IsSuccess)
        {
            _logger.LogError("Failed to delete encryption key. Id: {EncryptionKeyId}, Error: {Error}", encryptionKeyId, deleteEncryptionKey.Error);
            return Result.Fail($"Failed to delete encryption key with id {encryptionKeyId}.", FailureReason.Keyvault);
        }

        _logger.LogInformation("Successfully completed the deletion of SecureMessage and EncryptionKey. SecureMessageId: {SecureMessageId}, EncryptionKeyId: {EncryptionKeyId}", secureMessageId, encryptionKeyId);

        var secureMessageHistoryModel = new CreateSecureMessageHistoryModel { EncryptionKeyId = secureMessage.EncryptionKeyId, SecureMessageId = secureMessage.Id, CreatedBy = secureMessage.CreatedBy, Action = SecureMessageAction.Deleted, Recipient = secureMessage.Recipient, DeletedBy = "system", DeletedUtc = DateTime.UtcNow };
        await _historyCommand.CreateSecureMessageHistoryAsync(secureMessageHistoryModel);

        return Result.Success();
    }
}
