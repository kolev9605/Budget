﻿using Budget.Common;
using Budget.Core.Entities;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route(nameof(GetById))]
        public async Task<IActionResult> GetById(int categoryId)
            => Ok(await _categoryService.GetByIdAsync(categoryId, LoggedInUserId));

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
            => Ok(await _categoryService.GetAllAsync(LoggedInUserId));

        [HttpGet]
        [Route(nameof(GetAllPrimary))]
        public async Task<IActionResult> GetAllPrimary()
            => Ok(await _categoryService.GetAllPrimaryAsync(LoggedInUserId));

        [HttpGet]
        [Route(nameof(GetAllSubcategories))]
        public async Task<IActionResult> GetAllSubcategories(int parentCategoryId)
            => Ok(await _categoryService.GetAllSubcategoriesByParentCategoryIdAsync(parentCategoryId, LoggedInUserId));

        [HttpGet]
        [Route(nameof(GetCategoryTypes))]
        public IActionResult GetCategoryTypes()
            => Ok(EnumHelpers.GetListFromEnum<CategoryType>());

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create(CreateCategoryModel model)
            => Ok(await _categoryService.CreateAsync(model, LoggedInUserId));
    }
}
