using EStore.Application.Brands.Commands.CreateBrand;
using EStore.Application.Brands.Commands.DeleteBrand;
using EStore.Application.Brands.Commands.UpdateBrand;
using EStore.Application.Brands.Queries.GetBrandById;
using EStore.Application.Brands.Queries.GetBrandListPaged;
using EStore.Contracts.Brands;
using EStore.Domain.BrandAggregate.ValueObjects;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class BrandsController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public BrandsController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetBrands()
    {
        var brands = await _mediator.Send(new GetBrandListPagedQuery());

        return Ok(_mapper.Map<List<BrandResponse>>(brands));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBrand(Guid id)
    {
        var query = new GetBrandByIdQuery(BrandId.Create(id));
        var getBrandResult = await _mediator.Send(query);

        return getBrandResult.Match(
            brand => Ok(_mapper.Map<BrandResponse>(brand)),
            errors => Problem(errors));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrand(CreateBrandRequest request)
    {
        var command = _mapper.Map<CreateBrandCommand>(request);
        var createBrandResult = await _mediator.Send(command);

        return createBrandResult.Match(
            brand => CreatedAtGetBrand(_mapper.Map<BrandResponse>(brand)),
            errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBrand(Guid id, [FromBody] UpdateBrandRequest request)
    {
        var command = new UpdateBrandCommand(BrandId.Create(id), request.Name);
        var updateBrandResult = await _mediator.Send(command);

        return updateBrandResult.Match(
            updated => NoContent(),
            errors => Problem(errors));
    }

    [HttpDelete("{id:guid}")]

    public async Task<IActionResult> DeleteBrand(Guid id)
    {
        var command = new DeleteBrandCommand(BrandId.Create(id));
        var deleteBrandResult = await _mediator.Send(command);

        return deleteBrandResult.Match(
            deleted => NoContent(),
            errors => Problem(errors));
    }

    private CreatedAtActionResult CreatedAtGetBrand(BrandResponse brand)
    {
        return CreatedAtAction(
            actionName: nameof(GetBrand),
            routeValues: new { id = brand.Id },
            value: brand);
    }
}
