using Securely.Application.Results;

namespace Securely.Application.Commands;

public interface IDeleteSecureMessageCommand
{
    Task<Result> DeleteSecureMessageAsync(Guid id);
}

