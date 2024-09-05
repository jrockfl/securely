using Microsoft.Extensions.Logging;

using Securely.Application.Commands;
using Securely.Application.Models;
using Securely.Application.Queries;
using Securely.Domain.Enums;
using Securely.Infrastructure.ServiceBus;

namespace Securely.Application.Handlers;

public class ExpireEventHandler : IEventHandler
{
    private readonly ILogger<ExpireEventHandler> _logger;
    private readonly IGetExpiredSecureMessagesQuery _getExpiredSecureMessagesQuery;
    private readonly IDeleteSecureMesssageAndEncryptionKeyCommand _deleteSecureMesssageAndEncryptionKeyCommand;
    private readonly ICreateSecureMessageHistoryCommand _createSecureMessageHistoryCommand;

    public EventType EventType => EventType.Expire;

    public ExpireEventHandler(ILogger<ExpireEventHandler> logger, IGetExpiredSecureMessagesQuery getExpiredSecureMessagesQuery, IDeleteSecureMesssageAndEncryptionKeyCommand deleteSecureMesssageAndEncryptionKeyCommand, ICreateSecureMessageHistoryCommand createSecureMessageHistoryCommand)
    {
        _logger = logger;
        _getExpiredSecureMessagesQuery = getExpiredSecureMessagesQuery;
        _deleteSecureMesssageAndEncryptionKeyCommand = deleteSecureMesssageAndEncryptionKeyCommand;
        _createSecureMessageHistoryCommand = createSecureMessageHistoryCommand;
    }

    public async Task HandleASync(IEventMessage eventMessage)
    {
        _logger.LogInformation("Starting to handle expired messages event.");

        var expiredSecureMessages = await _getExpiredSecureMessagesQuery.GetExpireSecureMessagesAsync();

        if (!expiredSecureMessages.Value.Any())
        {
            _logger.LogInformation("No expired secure messages found to process.");
            return;
        }

        foreach (var expiredSecureMessage in expiredSecureMessages.Value)
        {
            try
            {
                var historyModel = new CreateSecureMessageHistoryModel
                {
                    Action = SecureMessageAction.Expired,
                    CreatedBy = expiredSecureMessage.CreatedBy,
                    EncryptionKeyId = expiredSecureMessage.EncryptionKeyId,
                    Recipient = expiredSecureMessage.Recipient,
                    SecureMessageId = expiredSecureMessage.Id
                };

                _logger.LogInformation($"Creating history entry for message with Id {expiredSecureMessage.Id}.");
                await _createSecureMessageHistoryCommand.CreateSecureMessageHistoryAsync(historyModel);

                _logger.LogInformation($"Attempting to delete message with Id {expiredSecureMessage.Id} and encryption key Id {expiredSecureMessage.EncryptionKeyId} that expired on {expiredSecureMessage.ExpirationDate}.");
                await _deleteSecureMesssageAndEncryptionKeyCommand.ExecuteAsync(expiredSecureMessage.Id, expiredSecureMessage.EncryptionKeyId);

                _logger.LogInformation($"Successfully processed message with Id {expiredSecureMessage.Id} and encryption key Id {expiredSecureMessage.EncryptionKeyId} that expired on {expiredSecureMessage.ExpirationDate}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to process expired message with Id {expiredSecureMessage.Id} and encryption key Id {expiredSecureMessage.EncryptionKeyId} due to an unexpected error.");
            }            
        }

        _logger.LogInformation("Finished handling expired messages event.");
    }
}
