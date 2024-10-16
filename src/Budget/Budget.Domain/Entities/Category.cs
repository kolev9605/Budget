using Budget.Domain.Entities.Base;

namespace Budget.Domain.Entities;

public class Category : BaseEntity, ICreatable, IUpdatable
{
    public string Name { get; set; } = null!;

    public ICollection<Record> Records { get; set; } = new List<Record>();

    public CategoryType CategoryType { get; set; }

    public Guid? ParentCategoryId { get; set; }

    public Category? ParentCategory { get; set; }

    public ICollection<Category> SubCategories { get; set; } = new List<Category>();

    public ICollection<UserCategory> Users { get; set; } = new List<UserCategory>();

    public bool IsInitial { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
