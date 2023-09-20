using Budget.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Domain.Models.Categories;

public class CategoryModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public CategoryType CategoryType { get; set; }

    public int? ParentCategoryId { get; set; }

    public bool IsInitial { get; set; }

    public IEnumerable<CategoryModel> SubCategories { get; set; } = new List<CategoryModel>();
}
