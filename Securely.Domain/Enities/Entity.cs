using System.ComponentModel.DataAnnotations;

namespace Securely.Domain.Enities;
public class Entity : IEntity
{
    public Entity()
    {
        Id = Guid.NewGuid();
        CreatedUtc = DateTime.UtcNow;
        UpdatedUtc = DateTime.UtcNow;
    }

    public Guid Id { get; set; }

    [Required]
    public DateTime CreatedUtc { get; set; }

    [Required]
    public DateTime UpdatedUtc { get; set; }
}
