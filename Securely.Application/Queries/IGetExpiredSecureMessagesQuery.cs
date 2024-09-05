using Securely.Application.Models;
using Securely.Application.Results;

namespace Securely.Application.Queries;

public interface IGetExpiredSecureMessagesQuery
{
    Task<Result<IEnumerable<SecureMessageModel>>> GetExpireSecureMessagesAsync();
}
