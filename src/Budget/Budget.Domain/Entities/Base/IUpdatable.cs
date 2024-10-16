namespace Budget.Domain.Entities.Base;

public interface IUpdatable
{
    public DateTimeOffset UpdatedAt { get; set; }
}
