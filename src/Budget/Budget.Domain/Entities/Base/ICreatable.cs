namespace Budget.Domain.Entities.Base;

public interface ICreatable
{
    public DateTimeOffset CreatedOn { get; set; }
}
