using Budget.Api.Models.Accounts;
using Budget.Api.Models.Categories;
using Budget.Application.Categories.Commands.Create;
using Budget.Application.Categories.Commands.Delete;
using Budget.Application.Categories.Commands.Update;
using Budget.Application.Categories.Queries;
using Budget.Application.Categories.Queries.GetById;
using Budget.Common;
using Budget.Domain.Entities;
using Budget.Domain.Models.Categories;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.Api.Controllers;

public class CategoriesController : BaseController
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var getCategoryByIdRequest = new GetCategoryByIdRequest(id);
        var result = await _mediator.Send((getCategoryByIdRequest, CurrentUser).Adapt<GetCategoryByIdQuery>());

        return MatchResponse<CategoryModel, CategoryResponse>(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCategoriesRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<GetAllCategoriesQuery>());

        return MatchResponse<IEnumerable<CategoryModel>, IEnumerable<CategoryResponse>>(result);
    }

    [HttpGet("types")]
    public IActionResult GetCategoryTypes()
        => Ok(EnumHelpers.GetListFromEnum<CategoryType>());

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<CreateCategoryCommand>());

        return MatchResponse<CategoryModel, CategoryResponse>(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var request = new DeleteCategoryRequest(id);
        var result = await _mediator.Send((request, CurrentUser).Adapt<DeleteCategoryCommand>());

        return MatchResponse<CategoryModel, CategoryResponse>(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateCategoryRequest request)
    {
        var result = await _mediator.Send((request, CurrentUser).Adapt<UpdateCategoryCommand>());

        return MatchResponse<CategoryModel, CategoryResponse>(result);
    }

    [HttpGet("most-used")]
    public async Task<IActionResult> GetMostUsedCategories([FromQuery] GetMostUsedCategoriesRequest request)
    {
        var query = new GetMostUsedCategoriesQuery(CurrentUser.Id, request.Count);
        var result = await _mediator.Send(query);

        return MatchResponse<IEnumerable<GetMostUsedCategoriesResult>, IEnumerable<GetMostUsedCategoriesResult>>(result);
    }
}
