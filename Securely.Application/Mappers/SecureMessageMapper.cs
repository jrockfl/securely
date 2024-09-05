using System.Text;

using Securely.Application.Models;
using Securely.Domain.Enities;

namespace Securely.Application.Mappers;

internal static class SecureMessageMapper
{
    internal static SecureMessageModel Map(SecureMessage secureMessage)
    {
        return new SecureMessageModel
        {
            Id = secureMessage.Id,
            Message = secureMessage.Message == null ? null : Encoding.UTF8.GetString(secureMessage.Message),
            CreatedBy = secureMessage.CreatedBy,
            Recipient = secureMessage.Recipient,            
            EncryptionKeyId = secureMessage.EncryptionKeyId,
            ExpirationDate = secureMessage.ExpirationUtc
        };
    }
}
