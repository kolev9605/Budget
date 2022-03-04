using Budget.Core.Entities;

namespace Budget.Core.Models.Categories
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CategoryType CategoryType { get; set; }
    }
}
