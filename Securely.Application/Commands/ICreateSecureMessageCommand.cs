using Securely.Application.Models;
using Securely.Application.Results;

namespace Securely.Application.Commands;
public interface ICreateSecureMessageCommand
{
    Task<Result<SecureMessageModel>> CreateSecureMessageAsync(CreateSecureMessageModel model);
}
