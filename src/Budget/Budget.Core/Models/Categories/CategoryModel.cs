using Budget.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Core.Models.Categories
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public CategoryType CategoryType { get; set; }

        public int? ParentCategoryId { get; set; }

        public IEnumerable<CategoryModel> SubCategories { get; set; } = new List<CategoryModel>();

        public static CategoryModel FromCategory(Category category)
        {
            return new CategoryModel()
            {
                Id = category.Id,
                Name = category.Name,
                CategoryType = category.CategoryType,
                ParentCategoryId = category.ParentCategoryId,
                SubCategories = category.SubCategories.Select(c => MapSubCategory(c))
            };
        }

        private static CategoryModel MapSubCategory(Category category)
        {
            var subCategoryModel = new CategoryModel()
            {
                Id = category.Id,
                Name = category.Name,
                CategoryType = category.CategoryType,
                ParentCategoryId = category.ParentCategoryId
            };

            return subCategoryModel;
        }
    }
}
