using EStore.Api.Common.ApiRoutes;
using EStore.Application.Discounts.Commands.CreateDiscount;
using EStore.Application.Discounts.Commands.UpdateDiscount;
using EStore.Application.Discounts.Queries.GetDiscountByIdQuery;
using EStore.Application.Discounts.Queries.GetDiscountListPaged;
using EStore.Contracts.Discounts;
using EStore.Infrastructure.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

[Authorize(Roles = $"{Roles.Admin}")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class DiscountsController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public DiscountsController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetDiscounts(int pageSize, int page)
    {
        var query = new GetDiscountListPagedQuery(pageSize, page);
        var getDiscountsResult = await _mediator.Send(query);

        return Ok(getDiscountsResult);
    }

    [HttpGet(ApiRoutes.Discount.Get)]
    public async Task<IActionResult> GetDiscount(Guid id)
    {
        var query = _mapper.Map<GetDiscountByIdQuery>(id);
        var getDiscountResult = await _mediator.Send(query);

        return getDiscountResult.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountCommand request)
    {
        var command = _mapper.Map<CreateDiscountCommand>(request);
        var createDiscountResult = await _mediator.Send(command);

        return createDiscountResult.Match(
            discount => CreatedAtGetDiscount(_mapper.Map<DiscountResponse>(discount)),
            Problem);
    }

    [HttpPut(ApiRoutes.Discount.Update)]
    public async Task<IActionResult> UpdateDiscount(Guid id, [FromBody] UpdateDiscountRequest request)
    {
        var command = _mapper.Map<UpdateDiscountCommand>((id, request));
        var updateDiscountResult = await _mediator.Send(command);

        return updateDiscountResult.Match(
            updated => NoContent(),
            Problem);
    }

    private CreatedAtActionResult CreatedAtGetDiscount(DiscountResponse discount)
    {
        return CreatedAtAction(
            actionName: nameof(GetDiscount),
            routeValues: new { id = discount.Id },
            value: discount);
    }
}
