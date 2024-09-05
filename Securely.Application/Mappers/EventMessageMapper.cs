using System.Text.Json;

using Securely.Application.Requests;
using Securely.Infrastructure.ServiceBus;

namespace Securely.Application.Mappers;

public static class EventMessageMapper
{
    public static IEventMessage Map(CleanupRequest cleanupRequest)
    {
        var eventMessage = new EventMessage { EventType = EventType.Cleanup, Body = JsonSerializer.Serialize(cleanupRequest) };

        return eventMessage;
    }
}
