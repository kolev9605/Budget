using Budget.Core.Entities;
using Budget.Core.Models.Categories;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Infrastructure.Factories
{
    public static class CategoryFactory
    {
        public static Category ToCategory(this CreateCategoryModel createCategoryModel, string userId)
        {
            if (createCategoryModel == null) return null;

            var category = new Category()
            {
                CategoryType = createCategoryModel.CategoryType,
                Name = createCategoryModel.Name,
                ParentCategoryId = createCategoryModel.ParentCategoryId
            };

            category.Users.Add(new UserCategory()
            {
                UserId = userId
            });

            return category;
        }

        public static CategoryModel ToCategoryModel(this Category category)
        {
            if (category == null) return null;

            var categoryModel = new CategoryModel()
            {
                Id = category.Id,
                Name = category.Name,
                CategoryType = category.CategoryType,
                ParentCategoryId = category.ParentCategoryId,
                SubCategories = category.SubCategories.Select(c => MapSubCategory(c)),
                IsInitial = category.IsInitial
            };

            return categoryModel;
        }

        public static IEnumerable<CategoryModel> ToCategoryModels(this IEnumerable<Category> categories)
        {
            if (categories == null) return Enumerable.Empty<CategoryModel>();

            return categories.Select(c => c.ToCategoryModel());
        }

        private static CategoryModel MapSubCategory(Category category)
        {
            var subCategoryModel = new CategoryModel()
            {
                Id = category.Id,
                Name = category.Name,
                CategoryType = category.CategoryType,
                ParentCategoryId = category.ParentCategoryId,
                IsInitial = category.IsInitial
            };

            return subCategoryModel;
        }
    }
}