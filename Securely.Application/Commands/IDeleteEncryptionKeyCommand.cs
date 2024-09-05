using Securely.Application.Results;

namespace Securely.Application.Commands;
public interface IDeleteEncryptionKeyCommand
{
    Task<Result> DeleteEncryptionKeyAsync(Guid id);
}
