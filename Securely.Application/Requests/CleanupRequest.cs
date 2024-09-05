namespace Securely.Application.Requests;

public class CleanupRequest
{
    public Guid SecureMessageId { get; set; }

    public Guid EncryptionKeyId { get; set; }
}
