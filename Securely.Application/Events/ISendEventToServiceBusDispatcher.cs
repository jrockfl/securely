using Securely.Application.Results;
using Securely.Infrastructure.ServiceBus;

namespace Securely.Application.Events;

public interface ISendEventToServiceBusDispatcher
{
    Task<Result> ExecuteAsync(IEventMessage model);
}
