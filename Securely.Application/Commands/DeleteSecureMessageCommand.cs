using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Securely.Application.Helpers;
using Securely.Application.Models;
using Securely.Application.Results;
using Securely.Domain.Enities;
using Securely.Infrastructure.Repositories;

namespace Securely.Application.Commands;

public class DeleteSecureMessageCommand : IDeleteSecureMessageCommand
{
    private readonly ICommandRepository _repository;
    private readonly ILogger<DeleteSecureMessageCommand> _logger;

    public DeleteSecureMessageCommand(ICommandRepository repository, ILogger<DeleteSecureMessageCommand> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result> DeleteSecureMessageAsync(Guid id)
    {
        Argument.AssertNotDefault(id, nameof(id));
        _logger.LogInformation($"Retrieving secure message with id {id} from the database.");

        var secureMessage = await _repository.Query<SecureMessage>().FirstOrDefaultAsync(x => x.Id == id);

        if (secureMessage == null)
        {
            _logger.LogWarning($"The secure message with id {id} does not exist in the database.");
            return Result<SecureMessageModel>.Fail($"The secure message with id {id} does not exist.", FailureReason.NotFound);
        }

        _repository.Delete(secureMessage);

        await _repository.SaveChangesAsync();

        _logger.LogInformation($"Secure message with id {id} has been deleted from the database successfully.");

        return Result.Success();
    }
}
