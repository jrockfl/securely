using Securely.Application.Models;
using Securely.Domain.Enities;

namespace Securely.Application.Mappers;

internal static class EncryptionKeyMapper
{
    internal static EncryptionKeyModel Map(EncryptionKey encryptionKey, string key)
    {
        return new EncryptionKeyModel
        {
            Id = encryptionKey.Id,
            Key = key
        };
    }
}
