using Securely.Infrastructure.ServiceBus;

namespace Securely.Application.Handlers;

public interface IEventHandler
{
    EventType EventType { get; }

    Task HandleASync(IEventMessage eventMessage);
}
