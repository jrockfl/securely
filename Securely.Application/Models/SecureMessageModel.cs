namespace Securely.Application.Models;

public class SecureMessageModel
{
    public Guid Id { get; set; }

    public Guid EncryptionKeyId { get; set; }

    public string Message { get; set; }

    public string Recipient { get; set; }

    public string CreatedBy { get; set; }

    public DateTime ExpirationDate { get; set; }
}
