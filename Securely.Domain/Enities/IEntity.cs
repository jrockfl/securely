namespace Securely.Domain.Enities;
public interface IEntity
{
    Guid Id { get; set; }

    DateTime CreatedUtc { get; set; }

    DateTime UpdatedUtc { get; set; }
}
