namespace Securely.Application.Models;

public class UpdateSecureMessageModel
{
    public Guid Id { get; set; }

    public Guid EncryptionKeyId { get; set; }

    public string Message { get; set; }

    public string CreatedBy { get; set; }

    public string DeletedBy { get; set; }

    public DateTime ExpirationDate { get; set; }
}
