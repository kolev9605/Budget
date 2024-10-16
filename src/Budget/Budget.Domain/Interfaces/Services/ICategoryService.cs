﻿using Budget.Domain.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<CategoryModel> GetByIdAsync(Guid categoryId, string userId);

        Task<IEnumerable<CategoryModel>> GetAllAsync(string userId);

        Task<IEnumerable<CategoryModel>> GetAllPrimaryAsync(string userId);

        Task<IEnumerable<CategoryModel>> GetAllSubcategoriesByParentCategoryIdAsync(Guid parentCategoryId, string userId);

        Task<CategoryModel> CreateAsync(CreateCategoryModel createCategoryModel, string userId);

        Task<CategoryModel> DeleteAsync(Guid categoryId, string userId);

        Task<CategoryModel> UpdateAsync(UpdateCategoryModel updateCategoryModel, string userId);
    }
}
