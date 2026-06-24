namespace DentalSaaS.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public string TenantId { get; set; } = string.Empty; // Shadow-like property
}
