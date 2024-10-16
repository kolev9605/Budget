using Budget.Domain.Entities;

namespace Budget.Domain.Models.Categories;

public class BaseCrudCategoryModel
{
    public string Name { get; set; } = null!;

    public CategoryType CategoryType { get; set; }

    public Guid? ParentCategoryId { get; set; }
}
