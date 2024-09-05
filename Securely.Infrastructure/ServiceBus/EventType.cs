using System.ComponentModel;

namespace Securely.Infrastructure.ServiceBus;

public enum EventType
{
    [Description("Unknown")]
    Unknown = 0,

    [Description("Cleanup")]
    Cleanup = 1,

    [Description("Expire")]
    Expire = 2,
}
