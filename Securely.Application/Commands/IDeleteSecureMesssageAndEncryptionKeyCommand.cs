using Securely.Application.Results;

namespace Securely.Application.Commands;

public interface IDeleteSecureMesssageAndEncryptionKeyCommand
{
    Task<Result> ExecuteAsync(Guid secureMessageId, Guid encryptionKeyId);
}
