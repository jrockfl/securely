using System.Text;

using Microsoft.Extensions.Logging;

using Securely.Application.Helpers;
using Securely.Application.Mappers;
using Securely.Application.Models;
using Securely.Application.Results;
using Securely.Domain.Enities;
using Securely.Domain.Enums;
using Securely.Infrastructure.Repositories;

namespace Securely.Application.Commands;

public class CreateSecureMessageCommand : ICreateSecureMessageCommand
{
    private readonly ICommandRepository _repository;
    private readonly ILogger<CreateSecureMessageCommand> _logger;
    private readonly ICreateSecureMessageHistoryCommand _historyCommand;

    public CreateSecureMessageCommand(ICommandRepository repository, ILogger<CreateSecureMessageCommand> logger, ICreateSecureMessageHistoryCommand historyCommand)
    {
        _repository = repository;
        _logger = logger;
        _historyCommand = historyCommand;
    }

    public async Task<Result<SecureMessageModel>> CreateSecureMessageAsync(CreateSecureMessageModel model)
    {
        Argument.AssertNotNull(model, nameof(model));
        Argument.AssertNotNullOrEmpty(model.SecureMessage, nameof(model.SecureMessage));
        Argument.AssertNotNullOrEmpty(model.CreatedBy, nameof(model.CreatedBy));
        Argument.AssertNotNullOrEmpty(model.Recipient, nameof(model.Recipient));
        Argument.AssertNotDefault(model.EncryptionKeyId, nameof(model.EncryptionKeyId));
        Argument.AssertNotNull(model.ExpirationDate, nameof(model.ExpirationDate));

        var encryptedData = Encoding.UTF8.GetBytes(model.SecureMessage);
        var secureMessage = new SecureMessage { EncryptionKeyId = model.EncryptionKeyId, Message = encryptedData, CreatedBy = model.CreatedBy, Recipient = model.Recipient, ExpirationUtc = model.ExpirationDate };

        _logger.LogInformation("Saving secure message to database.");

        _repository.Add(secureMessage);

        await _repository.SaveChangesAsync();

        _logger.LogInformation("Saved secure message to database successfully.");

        var secureMessageHistoryModel = new CreateSecureMessageHistoryModel { EncryptionKeyId = model.EncryptionKeyId, SecureMessageId = secureMessage.Id, CreatedBy = model.CreatedBy, Action = SecureMessageAction.Created, Recipient = model.Recipient };
        await _historyCommand.CreateSecureMessageHistoryAsync(secureMessageHistoryModel);

        var result = SecureMessageMapper.Map(secureMessage);

        return Result<SecureMessageModel>.Success(result);
    }
}
