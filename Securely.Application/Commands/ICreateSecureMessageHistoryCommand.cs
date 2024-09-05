using Securely.Application.Models;
using Securely.Application.Results;

namespace Securely.Application.Commands;

public interface ICreateSecureMessageHistoryCommand
{
    Task<Result> CreateSecureMessageHistoryAsync(CreateSecureMessageHistoryModel model);
}
