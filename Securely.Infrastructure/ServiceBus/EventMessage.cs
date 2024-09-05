
namespace Securely.Infrastructure.ServiceBus;

public class EventMessage : IEventMessage
{
    public EventType EventType { get; set; }

    public string Body { get; set; }

    public Guid Id => Guid.NewGuid();
}
