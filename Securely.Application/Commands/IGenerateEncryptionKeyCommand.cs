using Securely.Application.Models;
using Securely.Application.Results;

namespace Securely.Application.Commands;
public interface IGenerateEncryptionKeyCommand
{
    Task<Result<EncryptionKeyModel>> GenerateEncryptionKeyAsync();
}
