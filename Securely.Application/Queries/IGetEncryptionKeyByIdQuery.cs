using Securely.Application.Models;
using Securely.Application.Results;

namespace Securely.Application.Queries;

public interface IGetEncryptionKeyByIdQuery
{
    Task<Result<EncryptionKeyModel>> GetEncryptionKeyByIdAsync(Guid id);
}
