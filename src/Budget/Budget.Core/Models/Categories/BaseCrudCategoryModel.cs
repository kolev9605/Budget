using Budget.Core.Entities;

namespace Budget.Core.Models.Categories
{
    public class BaseCrudCategoryModel
    {
        public string Name { get; set; }

        public CategoryType CategoryType { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}