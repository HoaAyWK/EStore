using ErrorOr;
using EStore.Application.Categories.Commands.CreateCategory;
using EStore.Application.Categories.Commands.DeleteCategory;
using EStore.Application.Categories.Commands.UpdateCategory;
using EStore.Application.Categories.Queries.GetCategoryById;
using EStore.Application.Categories.Queries.GetCategoryListPaged;
using EStore.Application.Categories.Queries.GetParentCategoryWithChildren;
using EStore.Contracts.Categories;
using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class CategoriesController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public CategoriesController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var query = new GetCategoryListPagedQuery();
        var categories = await _mediator.Send(query);

        return Ok(_mapper.Map<List<CategoryResponse>>(categories));
    }

    [HttpGet("parents")]
    public async Task<IActionResult> GetParentCategory()
    {
        var query = new GetParentCategoryWithChildrenQuery();
        var categories = await _mediator.Send(query);

        return Ok(_mapper.Map<List<CategoryResponse>>(categories));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var query = new GetCategoryByIdQuery(id);
        var getCategoryResult = await _mediator.Send(query);

        return getCategoryResult.Match(
            category => Ok(_mapper.Map<CategoryResponse>(category)),
            errors => Problem(errors));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
    {
        if (request.ParentId != null && !Guid.TryParse(request.ParentId, out _))
        {
            return Problem(new List<Error>{ Errors.Category.ParentCategoryNotFound });
        }

        var command = new CreateCategoryCommand(
            request.Name,
            request.ParentId != null ? CategoryId.Create(new Guid(request.ParentId)) : null);

        var createCategoryResult = await _mediator.Send(command);

        return createCategoryResult.Match(
            category => CreatedAtGetCategory(_mapper.Map<CategoryResponse>(category)),
            errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryRequest request)
    {
        if (request.ParentId != null && !Guid.TryParse(request.ParentId, out _))
        {
            return Problem(new List<Error>{ Errors.Category.ParentCategoryNotFound });
        }

        var command = new UpdateCategoryCommand(
            CategoryId.Create(id),
            request.Name,
            request.ParentId != null ? CategoryId.Create(new Guid(request.ParentId)) : null);
        
        var updateCategoryResult = await _mediator.Send(command);

        return updateCategoryResult.Match(
            updated => NoContent(),
            errors => Problem(errors));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var command = new DeleteCategoryCommand(CategoryId.Create(id));
        var deleteCategoryResult = await _mediator.Send(command);

        return deleteCategoryResult.Match(
            deleted => NoContent(),
            errors => Problem(errors));
    }

    private CreatedAtActionResult CreatedAtGetCategory(CategoryResponse category)
    {
        return CreatedAtAction(
            actionName: nameof(GetCategory),
            routeValues: new { id = category.Id },
            value: category);
    }
}
