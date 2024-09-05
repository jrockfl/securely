using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Securely.Application.Mappers;
using Securely.Application.Models;
using Securely.Application.Results;
using Securely.Domain.Enities;
using Securely.Infrastructure.Repositories;

namespace Securely.Application.Queries;
public class GetExpiredSecureMessagesQuery : IGetExpiredSecureMessagesQuery
{
    private readonly ILogger<GetExpiredSecureMessagesQuery> _logger;
    private readonly IQueryRepository _repository;

    public GetExpiredSecureMessagesQuery(ILogger<GetExpiredSecureMessagesQuery> logger, IQueryRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<IEnumerable<SecureMessageModel>>> GetExpireSecureMessagesAsync()
    {
        _logger.LogInformation($"Retrieving expired secure messages from database.");

        var secureMessages = await _repository.Query<SecureMessage>().Where(x => x.ExpirationUtc <= DateTime.UtcNow).ToListAsync();

        if(!secureMessages.Any())
        {
            _logger.LogInformation("No secure messages were found to be marked as expired.");
            return Result<IEnumerable<SecureMessageModel>>.Success(Enumerable.Empty<SecureMessageModel>());
        }

        var secureMessageModels = secureMessages.Select(sm => SecureMessageMapper.Map(sm)).ToList();

        _logger.LogInformation($"{secureMessageModels.Count} secure message(s) are to be marked as expired.");

        return Result<IEnumerable<SecureMessageModel>>.Success(secureMessageModels);
    }
}
