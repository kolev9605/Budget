namespace Budget.Domain.Entities.Base;

public interface ICreatable
{
    public DateTimeOffset CreatedAt { get; set; }
}
