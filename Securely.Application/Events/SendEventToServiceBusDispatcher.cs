using Securely.Application.Results;
using Securely.Infrastructure.ServiceBus;

namespace Securely.Application.Events;

public class SendEventToServiceBusDispatcher : ISendEventToServiceBusDispatcher
{
    private readonly IServiceBusManager _serviceBus;

    public SendEventToServiceBusDispatcher(IServiceBusManager serviceBus)
    {
        _serviceBus = serviceBus;
    }

    public async Task<Result> ExecuteAsync(IEventMessage model)
    {
        await _serviceBus.SendMessageAsync(model);

        return Result.Success();
    }
}
