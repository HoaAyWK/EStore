using EStore.Application.Categories.Commands.CreateCategory;
using EStore.Application.Categories.Queries.GetCategoryById;
using EStore.Contracts.Categories;
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

    [HttpGet("{id}")]
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
        var command = _mapper.Map<CreateCategoryCommand>(request);
        var createCategoryResult = await _mediator.Send(command);

        return createCategoryResult.Match(
            category => Ok(_mapper.Map<CategoryResponse>(category)),
            errors => Problem(errors));
    }
}
