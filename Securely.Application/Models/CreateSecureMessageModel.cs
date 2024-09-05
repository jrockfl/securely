namespace Securely.Application.Models;

public class CreateSecureMessageModel
{
    public Guid EncryptionKeyId { get; set; }

    public string SecureMessage { get; set; }

    public string CreatedBy { get; set; }

    public string Recipient { get; set; }

    public DateTime ExpirationDate { get; set; }
}
