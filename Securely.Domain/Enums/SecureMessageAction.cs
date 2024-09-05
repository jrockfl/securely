using System.ComponentModel;

namespace Securely.Domain.Enums;
public enum SecureMessageAction
{
    [Description("Unknown")]
    Unknown = 0,

    [Description("Created")]
    Created = 1,

    [Description("Read")]
    Read = 2,

    [Description("Deleted")]
    Deleted = 3,

    [Description("Expired")]
    Expired = 4
}
