using EStore.Api.Common.ApiRoutes;
using EStore.Application.Searching.Commands;
using EStore.Application.Searching.Queries.SearchProductsQuery;
using EStore.Contracts.Searching;
using EStore.Infrastructure.Authentication;
using EStore.Infrastructure.Services.AlgoliaSearch.Interfaces;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class SearchController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly IAlgoliaIndexSettingsService _algoliaIndexSettingsService;

    public SearchController(
        ISender mediator,
        IMapper mapper,
        IAlgoliaIndexSettingsService algoliaIndexSettingsService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _algoliaIndexSettingsService = algoliaIndexSettingsService;
    }

    [AllowAnonymous]
    [HttpGet(ApiRoutes.Search.SearchProducts)]
    public async Task<IActionResult> SearchProducts(
        [FromQuery] SearchProductsRequest request)
    {
        var query = _mapper.Map<SearchProductsQuery>(request);

        return Ok(await _mediator.Send(query));
    }

    [AllowAnonymous]
    [HttpGet(ApiRoutes.Search.GetAlgoliaIndexSettings)]
    public async Task<IActionResult> GetAlgoliaIndexSettings()
    {
        return Ok(await _algoliaIndexSettingsService.GetIndexSettingsAsync());
    }

    [Authorize(Roles = $"{Roles.Admin}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut(ApiRoutes.Search.RebuildProductVariant)]
    public async Task<IActionResult> RebuildProductVariant(
        [FromBody] RebuildProductVariantRequest request)
    {
        var command = _mapper.Map<RebuildProductVariantRequest, RebuildProductVariantCommand>(request);
        var rebuildResult = await _mediator.Send(command);

        return rebuildResult.Match(Ok, Problem);
    }
}
