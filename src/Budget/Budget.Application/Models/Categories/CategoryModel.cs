using Budget.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Application.Models.Categories
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CategoryType CategoryType { get; set; }

        public int? ParentCategoryId { get; set; }

        public bool IsInitial { get; set; }

        public IEnumerable<CategoryModel> SubCategories { get; set; } = new List<CategoryModel>();
    }
}
