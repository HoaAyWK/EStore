using EStore.Application.Brands.Commands.CreateBrand;
using EStore.Contracts.Brands;
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

    [HttpPost]
    public async Task<IActionResult> CreateBrand(CreateBrandRequest request)
    {
        var command = _mapper.Map<CreateBrandCommand>(request);
        var createBrandResult = await _mediator.Send(command);

        return createBrandResult.Match(
            brand => Ok(_mapper.Map<BrandResponse>(brand)),
            errors => Problem(errors));
    }
}
