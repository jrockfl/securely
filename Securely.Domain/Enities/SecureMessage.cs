using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Securely.Domain.Enities;
public class SecureMessage : Entity
{

    [ForeignKey("EncryptionKeyId")]
    public virtual EncryptionKey EncryptionKey { get; set; }

    [Required]
    public Guid EncryptionKeyId { get; set; }

    [Required]
    public byte[] Message { get; set; }

    [MaxLength(50)]
    [Required]
    public string CreatedBy { get; set; }

    [MaxLength(50)]
    [Required]
    public string Recipient { get; set; }

    [Required]
    public DateTime ExpirationUtc { get; set; }
}
