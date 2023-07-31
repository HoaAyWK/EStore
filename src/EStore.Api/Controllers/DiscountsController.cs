using EStore.Application.Discounts.Commands.CreateDiscount;
using EStore.Application.Discounts.Queries.GetDiscountByIdQuery;
using EStore.Contracts.Discounts;
using EStore.Domain.DiscountAggregate.ValueObjects;
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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDiscount(Guid id)
    {
        var query = _mapper.Map<GetDiscountByIdQuery>(id);
        var getDiscountResult = await _mediator.Send(query);

        return getDiscountResult.Match(
            discount => Ok(discount),
            errors => Problem(errors));
    }

    [HttpPost]
    public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountCommand request)
    {
        var command = _mapper.Map<CreateDiscountCommand>(request);
        var createDiscountResult = await _mediator.Send(command);

        return createDiscountResult.Match(
            discount => CreatedAtGetDiscount(_mapper.Map<DiscountResponse>(discount)),
            errors => Problem(errors));
    }

    private CreatedAtActionResult CreatedAtGetDiscount(DiscountResponse discount)
    {
        return CreatedAtAction(
            actionName: nameof(GetDiscount),
            routeValues: new { id = discount.Id },
            value: discount);
    }
}
