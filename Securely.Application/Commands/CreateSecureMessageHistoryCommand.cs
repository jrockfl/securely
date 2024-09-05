using Microsoft.Extensions.Logging;

using Securely.Application.Helpers;
using Securely.Application.Models;
using Securely.Application.Results;
using Securely.Domain.Enities;
using Securely.Infrastructure.Repositories;

namespace Securely.Application.Commands;
public class CreateSecureMessageHistoryCommand : ICreateSecureMessageHistoryCommand
{
    private readonly ICommandRepository _repository;
    private readonly ILogger<CreateSecureMessageHistoryCommand> _logger;

    public CreateSecureMessageHistoryCommand(ICommandRepository repository, ILogger<CreateSecureMessageHistoryCommand> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result> CreateSecureMessageHistoryAsync(CreateSecureMessageHistoryModel model)
    {
        Argument.AssertNotNull(model, nameof(model));
        Argument.AssertNotNullOrEmpty(model.CreatedBy, nameof(model.CreatedBy));
        Argument.AssertNotNullOrEmpty(model.Recipient, nameof(model.Recipient));
        Argument.AssertNotDefault(model.EncryptionKeyId, nameof(model.EncryptionKeyId));
        Argument.AssertNotDefault(model.SecureMessageId, nameof(model.SecureMessageId));

        var secureMessageHistory = new SecureMessageHistory { EncryptionKeyId = model.EncryptionKeyId, SecureMessageId = model.SecureMessageId, CreatedBy = model.CreatedBy, Action = model.Action, Recipient = model.Recipient, DeletedBy = model.DeletedBy, DeletedUtc = model.DeletedUtc };

        _logger.LogInformation("Saving secure message history to database.");

        _repository.Add(secureMessageHistory);

        await _repository.SaveChangesAsync();

        _logger.LogInformation("Saved secure message history to database successfully.");

        return Result.Success();
    }
}
