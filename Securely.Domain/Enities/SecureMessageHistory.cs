using System.ComponentModel.DataAnnotations;

using Securely.Domain.Enums;

namespace Securely.Domain.Enities;
public class SecureMessageHistory : Entity
{
    [Required]
    public Guid SecureMessageId { get; set; }

    [Required]
    public Guid EncryptionKeyId { get; set; }

    [MaxLength(50)]
    [Required]
    public string CreatedBy { get; set; }

    [MaxLength(50)]
    [Required]
    public string Recipient { get; set; }

    [Required]
    public SecureMessageAction Action { get; set; }

    [MaxLength(50)]
    public string DeletedBy { get; set; }

    public DateTime? DeletedUtc { get; set; }
}
