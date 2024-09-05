using System.Text.Json;

using Azure.Messaging.ServiceBus;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

using Securely.Application.Common;
using Securely.Application.Handlers;
using Securely.Infrastructure.ServiceBus;

namespace Securely.Functions.EventManager;

public class EventManagerFunction
{
    private readonly ILogger<EventManagerFunction> _logger;
    private readonly EventHandlerFactory _eventHandlerFactory;

    public EventManagerFunction(ILogger<EventManagerFunction> logger, EventHandlerFactory eventHandlerFactory)
    {
        _logger = logger;
        _eventHandlerFactory = eventHandlerFactory;
    }

    [Function(nameof(EventManagerFunction))]
    public async Task Run(
        [ServiceBusTrigger(Constants.ServiceBus.QueueName.Event, Connection = "ServiceBusConnectionString")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation($"Received message Id: {message.MessageId} on {nameof(EventManagerFunction)}.", message.MessageId);

        try
        {
            var eventMessage = JsonSerializer.Deserialize<EventMessage>(message.Body);

            if (eventMessage != null)
            {
                var handler = _eventHandlerFactory.GetHandler(eventMessage.EventType);
                await handler.HandleASync(eventMessage);
            }

            await messageActions.CompleteMessageAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred while processing message: {id}. Exception: {Exception}", message.MessageId, ex);
            await messageActions.AbandonMessageAsync(message);
        }
    }
}
