using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Securely.Application.Commands;
using Securely.Application.Helpers;
using Securely.Application.Mappers;
using Securely.Application.Models;
using Securely.Application.Results;
using Securely.Domain.Enities;
using Securely.Domain.Enums;
using Securely.Infrastructure.Repositories;

namespace Securely.Application.Queries;
public class GetSecureMessageByIdQuery : IGetSecureMessageByIdQuery
{
    private readonly IQueryRepository _repository;
    private readonly ILogger<GetEncryptionKeyByIdQuery> _logger;
    private readonly ICreateSecureMessageHistoryCommand _historyCommand;

    public GetSecureMessageByIdQuery(IQueryRepository repository, ILogger<GetEncryptionKeyByIdQuery> logger, ICreateSecureMessageHistoryCommand historyCommand)
    {
        _repository = repository;
        _logger = logger;
        _historyCommand = historyCommand;
    }

    public async Task<Result<SecureMessageModel>> GetSecureMessageByIdAsync(Guid id)
    {
        Argument.AssertNotDefault(id, nameof(id));
        _logger.LogInformation($"Retrieving secure message with id {id} from the database.");

        var secureMessage = await _repository.Query<SecureMessage>().FirstOrDefaultAsync(x => x.Id == id);

        if (secureMessage == null)
        {
            _logger.LogWarning($"The secure message with id {id} does not exist in the database.");
            return Result<SecureMessageModel>.Fail($"The secure message with id {id} does not exist.", FailureReason.NotFound);
        }

        if (secureMessage.Message == null)
        {
            _logger.LogWarning($"The secure message with id {id} has already been retried an no longer exists.");
            return Result<SecureMessageModel>.Fail($"The secure message with id {id} has already been retrieved.", FailureReason.Gone);
        }

        if (IsSecureMessageExpired(secureMessage))
        {
            _logger.LogWarning($"The secure message with id {id} has expired.");
            return Result<SecureMessageModel>.Fail($"The secure message with id {id} has expired.", FailureReason.Expired);
        }

        var secureMessageModel = SecureMessageMapper.Map(secureMessage);

        _logger.LogInformation($"Retrieved secure message with id {id} from the database successfully.");

        var secureMessageHistoryModel = new CreateSecureMessageHistoryModel
        {
            EncryptionKeyId = secureMessage.EncryptionKeyId,
            SecureMessageId = secureMessage.Id,
            CreatedBy = secureMessage.CreatedBy,
            Action = SecureMessageAction.Read,
            Recipient = secureMessage.Recipient
        };

        await _historyCommand.CreateSecureMessageHistoryAsync(secureMessageHistoryModel);

        return Result<SecureMessageModel>.Success(secureMessageModel);
    }

    private static bool IsSecureMessageExpired(SecureMessage secureMessage)
    {
        return secureMessage.ExpirationUtc < DateTime.UtcNow;
    }
}
