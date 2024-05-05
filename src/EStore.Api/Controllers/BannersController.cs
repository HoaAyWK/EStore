using EStore.Api.Common.ApiRoutes;
using EStore.Application.Banners.Commands.AddBanner;
using EStore.Application.Banners.Commands.UpdateBanner;
using EStore.Application.Banners.Queries.GetBannerById;
using EStore.Application.Banners.Queries.GetBanners;
using EStore.Contracts.Banners;
using EStore.Infrastructure.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

[Authorize(Roles = $"{Roles.Admin}")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BannersController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public BannersController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet(ApiRoutes.Banner.Get)]
    public async Task<IActionResult> GetBanner(Guid id)
    {
        var query = _mapper.Map<Guid, GetBannerByIdQuery>(id);
        var getBannerResult = await _mediator.Send(query);
        
        return getBannerResult.Match(Ok, Problem);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetBanners([FromQuery] GetBannersRequest request)
    {
        var query = _mapper.Map<GetBannersRequest, GetBannersQuery>(request);
        var getBannersResult = await _mediator.Send(query);

        return Ok(getBannersResult);
    }

    [HttpPost(ApiRoutes.Banner.Add)]
    public async Task<IActionResult> Add([FromBody] AddBannerRequest request)
    {
        var command = _mapper.Map<AddBannerRequest, AddBannerCommand>(request);
        var updateBannerResult = await _mediator.Send(command);

        if (updateBannerResult.IsError)
        {
            return Problem(updateBannerResult.Errors);
        }

        var bannerId = updateBannerResult.Value.Id.Value;
        var query = _mapper.Map<Guid, GetBannerByIdQuery>(bannerId);
        var getBannerResult = await _mediator.Send(query);

        return getBannerResult.Match(Ok, Problem);
    }

    [HttpPut(ApiRoutes.Banner.Update)]
    public async Task<IActionResult> Update([FromBody] UpdateBannerRequest request)
    {
        var command = _mapper.Map<UpdateBannerRequest, UpdateBannerCommand>(request);
        var updateBannerResult = await _mediator.Send(command);

        if (updateBannerResult.IsError)
        {
            return Problem(updateBannerResult.Errors);
        }

        var query = _mapper.Map<Guid, GetBannerByIdQuery>(request.BannerId);
        var getBannerResult = await _mediator.Send(query);

        return getBannerResult.Match(Ok, Problem);
    }
}
