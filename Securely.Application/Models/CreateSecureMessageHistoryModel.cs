using Securely.Domain.Enums;

namespace Securely.Application.Models;
public class CreateSecureMessageHistoryModel
{
    public Guid SecureMessageId { get; set; }

    public Guid EncryptionKeyId { get; set; }

    public string CreatedBy { get; set; }

    public string Recipient { get; set; }

    public SecureMessageAction Action { get; set; }

    public string DeletedBy { get; set; }

    public DateTime? DeletedUtc { get; set; }

}

