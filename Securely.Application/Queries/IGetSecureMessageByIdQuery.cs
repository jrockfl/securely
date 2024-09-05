using Securely.Application.Models;
using Securely.Application.Results;

namespace Securely.Application.Queries;

public interface IGetSecureMessageByIdQuery
{
    Task<Result<SecureMessageModel>> GetSecureMessageByIdAsync(Guid id);
}
