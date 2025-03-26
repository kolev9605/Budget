namespace Budget.Domain.Entities.Base;

public class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
