using Securely.Application.Models;
using Securely.Application.Results;

namespace Securely.Application.Commands;
public interface IUpdateSecureMessageCommand
{
    Task<Result<SecureMessageModel>> UpdateSecureMessageAsync(UpdateSecureMessageModel model);
}
