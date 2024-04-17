using EStore.Api.Common.ApiRoutes;
using EStore.Api.Common.Contexts;
using EStore.Application.Carts.Commands.AddItemToCart;
using EStore.Application.Carts.Commands.CreateCart;
using EStore.Application.Carts.Commands.RemoveItemFromCart;
using EStore.Application.Carts.Queries.GetCartByCustomerId;
using EStore.Contracts.Carts;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class CartsController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly IWorkContext _workContext;

    public CartsController(ISender mediator, IMapper mapper, IWorkContext workContext)
    {
        _mediator = mediator;
        _mapper = mapper;
        _workContext = workContext;
    }

    [HttpGet(ApiRoutes.Cart.GetMyCart)]
    public async Task<IActionResult> GetCustomerCart()
    {
        var customerId = _workContext.CustomerId;
        var query = _mapper.Map<GetCartByCustomerIdQuery>(customerId);
        var getCustomerCartResult = await _mediator.Send(query);

        if (getCustomerCartResult.IsError)
        {
            // Try create if cart doesn't exist
            var command = _mapper.Map<CreateCartCommand>(customerId);
            var createCartResult = await _mediator.Send(command);
            getCustomerCartResult = await _mediator.Send(query);
        }

        return getCustomerCartResult.Match(Ok, Problem);
    }

    [HttpPut]
    public async Task<IActionResult> AddItemToCart([FromBody] AddItemToCartRequest request)
    {
        var customerId = _workContext.CustomerId;
        var command = _mapper.Map<AddItemToCartCommand>((customerId, request));
        var addItemToCartResult = await _mediator.Send(command);

        if (addItemToCartResult.IsError)
        {
            return Problem(addItemToCartResult.Errors);
        }

        var query = _mapper.Map<GetCartByCustomerIdQuery>(customerId);
        var getCustomerCartResult = await _mediator.Send(query);

        return getCustomerCartResult.Match(Ok, Problem);
    }

    [HttpDelete(ApiRoutes.Cart.RemoveItem)]
    public async Task<IActionResult> RemoveItemFromCart([FromRoute] RemoveCartItemRequest request)
    {
        var command = _mapper.Map<RemoveItemFromCartCommand>(request);
        var removeItemFromCartResult = await _mediator.Send(command);

        return removeItemFromCartResult.Match(
            deleted => NoContent(),
            errors => Problem(errors));
    }
}
