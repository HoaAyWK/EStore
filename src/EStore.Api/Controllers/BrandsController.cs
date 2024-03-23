using EStore.Api.Common.ApiRoutes;
using EStore.Application.Brands.Commands.CreateBrand;
using EStore.Application.Brands.Commands.DeleteBrand;
using EStore.Application.Brands.Commands.UpdateBrand;
using EStore.Application.Brands.Queries.GetBrandById;
using EStore.Application.Brands.Queries.GetBrandListPaged;
using EStore.Contracts.Brands;
using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Infrastructure.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

[Authorize(Roles = $"{Roles.Admin}")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BrandsController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public BrandsController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetBrands(int pageSize, int page)
    {
        var listPagedBrand = await _mediator.Send(new GetBrandListPagedQuery(pageSize, page));

        return Ok(listPagedBrand);
    }

    [AllowAnonymous]
    [HttpGet(ApiRoutes.Brand.Get)]
    public async Task<IActionResult> GetBrand(Guid id)
    {
        var query = new GetBrandByIdQuery(BrandId.Create(id));
        var getBrandResult = await _mediator.Send(query);

        return getBrandResult.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrand(CreateBrandRequest request)
    {
        var command = _mapper.Map<CreateBrandCommand>(request);
        var createBrandResult = await _mediator.Send(command);

        return createBrandResult.Match(
            brand => CreatedAtGetBrand(_mapper.Map<BrandResponse>(brand)),
            Problem);
    }

    [HttpPut(ApiRoutes.Brand.Update)]
    public async Task<IActionResult> UpdateBrand(Guid id, [FromBody] UpdateBrandRequest request)
    {
        var command = _mapper.Map<UpdateBrandCommand>((id, request));
        var updateBrandResult = await _mediator.Send(command);

        if (updateBrandResult.IsError)
        {
            return Problem(updateBrandResult.Errors);
        }

        var query = new GetBrandByIdQuery(BrandId.Create(id));
        var getBrandResult = await _mediator.Send(query);

        return getBrandResult.Match(Ok, Problem);
    }

    [HttpDelete(ApiRoutes.Brand.Delete)]
    public async Task<IActionResult> DeleteBrand(Guid id)
    {
        var command = _mapper.Map<DeleteBrandCommand>(id);
        var deleteBrandResult = await _mediator.Send(command);

        return deleteBrandResult.Match(
            deleted => NoContent(),
            Problem);
    }

    private CreatedAtActionResult CreatedAtGetBrand(BrandResponse brand)
    {
        return CreatedAtAction(
            actionName: nameof(GetBrand),
            routeValues: new { id = brand.Id },
            value: brand);
    }
}
