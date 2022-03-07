using Budget.Core.Entities;

namespace Budget.Core.Models.Categories
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CategoryType CategoryType { get; set; }

        public static CategoryModel FromCategory(Category category)
        {
            return new CategoryModel()
            {
                Id = category.Id,
                Name = category.Name,
                CategoryType = category.CategoryType,
            };
        }
    }
}
