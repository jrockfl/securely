namespace Securely.Infrastructure.ServiceBus;

public interface IServiceBusManager
{
    Task SendMessageAsync<T>(T message) where T : IEventMessage;
}
