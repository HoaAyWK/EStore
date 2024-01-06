using EStore.Api.Common.ApiRoutes;
using EStore.Application.Searching.Queries.SearchProductsQuery;
using EStore.Contracts.Searching;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class SearchController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public SearchController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet(ApiRoutes.Search.SearchProducts)]
    public async Task<IActionResult> SearchProducts(
        [FromQuery] SearchProductsRequest request)
    {
        var query = _mapper.Map<SearchProductsQuery>(request);

        return Ok(await _mediator.Send(query));
    }
}
