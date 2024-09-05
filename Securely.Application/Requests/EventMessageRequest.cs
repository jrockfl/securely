using Securely.Infrastructure.ServiceBus;

namespace Securely.Application.Requests;
public class EventMessageRequest : IEventMessage
{
    public EventType EventType { get; set; }

    public string Body { get; set; }

    public Guid Id => Guid.NewGuid();
}
