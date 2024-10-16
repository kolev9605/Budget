namespace Budget.Domain.Entities.Base;

public interface IUpdatableEntity
{
    public DateTimeOffset UpdatedAt { get; set; }
}
