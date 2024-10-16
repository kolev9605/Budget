using Budget.Api.Models.Accounts;
using Budget.Api.Models.Categories;
using Budget.Application.Categories.Queries.GetById;
using Budget.Common;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Categories;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers;

public class CategoriesController : BaseController
{
    private readonly ICategoryService _categoryService;
    private readonly IMediator _mediator;

    public CategoriesController(ICategoryService categoryService, IMediator mediator)
    {
        _categoryService = categoryService;
        _mediator = mediator;
    }

    [HttpGet]
    [Route(nameof(GetById))]
    public async Task<IActionResult> GetById([FromQuery] GetCategoryByIdRequest getCategoryByIdRequest)
    {
        var result = await _mediator.Send((getCategoryByIdRequest, CurrentUser).Adapt<GetCategoryByIdQuery>());

        return MatchResponse<CategoryModel, CategoryResponse>(result);
    }

    [HttpGet]
    [Route(nameof(GetAll))]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _categoryService.GetAllAsync(CurrentUser.Id));
    }

    [HttpGet]
    [Route(nameof(GetAllPrimary))]
    public async Task<IActionResult> GetAllPrimary()
        => Ok(await _categoryService.GetAllPrimaryAsync(CurrentUser.Id));

    [HttpGet]
    [Route(nameof(GetAllSubcategories))]
    public async Task<IActionResult> GetAllSubcategories(Guid parentCategoryId)
        => Ok(await _categoryService.GetAllSubcategoriesByParentCategoryIdAsync(parentCategoryId, CurrentUser.Id));

    [HttpGet]
    [Route(nameof(GetCategoryTypes))]
    public IActionResult GetCategoryTypes()
        => Ok(EnumHelpers.GetListFromEnum<CategoryType>());

    [HttpPost]
    [Route(nameof(Create))]
    public async Task<IActionResult> Create(CreateCategoryModel model)
        => Ok(await _categoryService.CreateAsync(model, CurrentUser.Id));

    [HttpDelete]
    [Route(nameof(Delete))]
    public async Task<IActionResult> Delete(Guid categoryId)
        => Ok(await _categoryService.DeleteAsync(categoryId, CurrentUser.Id));

    [HttpPost]
    [Route(nameof(Update))]
    public async Task<IActionResult> Update(UpdateCategoryModel updateCategoryModel)
        => Ok(await _categoryService.UpdateAsync(updateCategoryModel, CurrentUser.Id));
}
