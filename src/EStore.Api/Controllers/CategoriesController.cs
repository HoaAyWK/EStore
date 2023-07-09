using ErrorOr;
using EStore.Application.Categories.Commands.CreateCategory;
using EStore.Application.Categories.Commands.DeleteCategory;
using EStore.Application.Categories.Commands.UpdateCategory;
using EStore.Application.Categories.Queries.GetAllCategories;
using EStore.Application.Categories.Queries.GetAllParentsWithChildren;
using EStore.Application.Categories.Queries.GetCategoryById;
using EStore.Application.Categories.Queries.GetCategoryListPaged;
using EStore.Application.Categories.Queries.GetCategoryTree;
// using EStore.Application.Categories.Queries.GetParentCategoryWithChildren;
using EStore.Contracts.Categories;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.Common.Errors;
using EStore.Infrastructure.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

[Authorize(Roles = $"{Roles.Admin}")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CategoriesController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public CategoriesController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetCategories(int pageSize, int page)
    {
        var query = new GetCategoryListPagedQuery(pageSize, page);
        var listPagedCategoryResult = await _mediator.Send(query);

        return Ok(_mapper.Map<ListPagedCategoryResponse>(listPagedCategoryResult));
    }

    [AllowAnonymous]
    [HttpGet("parents")]
    public async Task<IActionResult> GetParentCategory()
    {
        var query = new GetAllParentsWithChildrenQuery();
        var categories = await _mediator.Send(query);

        // return Ok(_mapper.Map<List<CategoryWithChildrenResponse>>(categories));

        return Ok(categories);
    }

    [AllowAnonymous]
    [HttpGet("tree")]
    public async Task<IActionResult> GetCategoryTree()
    {
        var query = new GetCategoryTreeQuery();
        var categoryTree = await _mediator.Send(query);
        
        return Ok(_mapper.Map<List<CategoryNodeResponse>>(categoryTree));
    }

    [AllowAnonymous]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllCategories()
    {
        var query = new GetAllCategoriesQuery();
        var categories = await _mediator.Send(query);

        return Ok(categories);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var query = new GetCategoryByIdQuery(CategoryId.Create(id));
        var getCategoryResult = await _mediator.Send(query);

        return getCategoryResult.Match(
            category => Ok(_mapper.Map<CategoryWithChildrenResponse>(category)),
            errors => Problem(errors));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
    {
        if (request.ParentId != null && !Guid.TryParse(request.ParentId, out _))
        {
            return Problem(new List<Error>{ Errors.Category.ParentCategoryNotFound });
        }

        var command = _mapper.Map<CreateCategoryCommand>(request);
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

        var command = _mapper.Map<UpdateCategoryCommand>((id, request));
        
        var updateCategoryResult = await _mediator.Send(command);

        return updateCategoryResult.Match(
            updated => NoContent(),
            errors => Problem(errors));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var command = _mapper.Map<DeleteCategoryCommand>(id);
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