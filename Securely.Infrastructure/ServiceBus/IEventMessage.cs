namespace Securely.Infrastructure.ServiceBus;

public interface IEventMessage
{
    Guid Id { get; }

    EventType EventType { get; }

    string Body { get; set; }
}
