using Securely.Application.Models;
using Securely.Application.Results;

namespace Securely.Application.Queries;
public interface IGetEncryptionKeyByMessgeIdQuery
{
    Task<Result<EncryptionKeyModel>> GetEncryptionKeyByMessageIdAsync(Guid messageId);
}
