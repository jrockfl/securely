using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Securely.Application.Helpers;
using Securely.Application.Mappers;
using Securely.Application.Models;
using Securely.Application.Results;
using Securely.Domain.Enities;
using Securely.Infrastructure.Repositories;

namespace Securely.Application.Commands;
public class UpdateSecureMessageCommand : IUpdateSecureMessageCommand
{
    private readonly ICommandRepository _repository;
    private readonly ILogger<UpdateSecureMessageCommand> _logger;

    public UpdateSecureMessageCommand(ICommandRepository repository, ILogger<UpdateSecureMessageCommand> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<SecureMessageModel>> UpdateSecureMessageAsync(UpdateSecureMessageModel model)
    {
        Argument.AssertNotNull(model, nameof(model));
        Argument.AssertNotDefault(model.Id, nameof(model.Id));

        _logger.LogInformation($"Retrieving secure message with id {model.Id} from the database.");

        var secureMessage = await _repository.Query<SecureMessage>().FirstOrDefaultAsync(x => x.Id == model.Id);

        if (secureMessage == null)
        {
            _logger.LogWarning($"The secure message with id {model.Id} does not exist in the database.");
            return Result<SecureMessageModel>.Fail($"The secure message with id {model.Id} does not exist.", FailureReason.NotFound);
        }

        secureMessage.Message = model.Message == null ? null : Encoding.UTF8.GetBytes(model.Message);
        secureMessage.ExpirationUtc = model.ExpirationDate;
        secureMessage.CreatedBy = model.CreatedBy;
        secureMessage.EncryptionKeyId = model.EncryptionKeyId;

        await _repository.SaveChangesAsync();

        _logger.LogInformation($"Updated secure message with id {model.Id} in the database successfully.");

        var result = SecureMessageMapper.Map(secureMessage);

        return Result<SecureMessageModel>.Success(result);
    }
}
