using System.Text.Json;

using Microsoft.Extensions.Logging;

using Securely.Application.Commands;
using Securely.Application.Requests;
using Securely.Infrastructure.ServiceBus;

namespace Securely.Application.Handlers;

public class CleanupEventHandler : IEventHandler
{
    private readonly ILogger<CleanupEventHandler> _logger;
    private readonly IDeleteSecureMesssageAndEncryptionKeyCommand _deleteSecureMesssageAndEncryptionKeyCommand;

    public EventType EventType => EventType.Cleanup;

    public CleanupEventHandler(IDeleteSecureMesssageAndEncryptionKeyCommand deleteSecureMesssageAndEncryptionKeyCommand, ILogger<CleanupEventHandler> logger)
    {
        _deleteSecureMesssageAndEncryptionKeyCommand = deleteSecureMesssageAndEncryptionKeyCommand;
        _logger = logger;
    }

    public async Task HandleASync(IEventMessage eventMessage)
    {
        var payload = JsonSerializer.Deserialize<CleanupRequest>(eventMessage.Body);

        if (payload != null)
        {
            var result = await _deleteSecureMesssageAndEncryptionKeyCommand.ExecuteAsync(payload.SecureMessageId, payload.EncryptionKeyId);

            if (result.IsSuccess)
            {
                _logger.LogInformation($"Processed eventMessage Id: {eventMessage.Id} on {nameof(CleanupEventHandler)} successfully.", eventMessage.Id);
            }
            else
            {
                _logger.LogError("Failed to process eventMessage: {id}. Error: {Error}", eventMessage.Id, result.Error);
            }
        }
        else
        {
            _logger.LogError("Failed to deserialize eventMessage body: {body}", eventMessage.Body.ToString());
        }
    }
}
