using Securely.Infrastructure.ServiceBus;

namespace Securely.Application.Handlers;
public class EventHandlerFactory
{
    private readonly Dictionary<EventType, IEventHandler> _handlers;

    public EventHandlerFactory(IEnumerable<IEventHandler> handlers)
    {
        _handlers = handlers.ToDictionary(handler => handler.EventType);
    }

    public IEventHandler GetHandler(EventType eventType)
    {
        if (_handlers.TryGetValue(eventType, out var handler))
        {
            return handler;
        }

        throw new ArgumentException($"{eventType} is not registered.");
    }
}
