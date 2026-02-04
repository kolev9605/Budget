namespace Budget.Api.Domain.Entities.Base;

public interface IUpdatable
{
    public DateTimeOffset UpdatedOn { get; set; }
}
