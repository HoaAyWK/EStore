using EStore.Api.Common.ApiRoutes;
using EStore.Application.Customers.Command.UpdateCustomer;
using EStore.Application.Customers.Commands.AddAddress;
using EStore.Application.Customers.Commands.UpdateAddress;
using EStore.Application.Customers.Queries.GetCustomerById;
using EStore.Contracts.Common;
using EStore.Contracts.Customers;
using EStore.Domain.CustomerAggregate.Entities;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class CustomersController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public CustomersController(
        ISender mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPut(ApiRoutes.Customer.Update)]
    public async Task<IActionResult> UpdateCustomer(
        Guid id,
        [FromBody] UpdateCustomerRequest request)
    {
        var command = _mapper.Map<(Guid, UpdateCustomerRequest), UpdateCustomerCommand>(
            (id, request));

        var updateCustomerResult = await _mediator.Send(command);

        return updateCustomerResult.Match(
            updated => NoContent(),
            Problem);
    }

    [HttpGet(ApiRoutes.Customer.Get)]
    public async Task<IActionResult> GetCustomer(Guid id)
    {
        var getCustomerResult = await _mediator.Send(
            new GetCustomerByIdQuery(CustomerId.Create(id)));

        return getCustomerResult.Match(Ok, Problem);
    }

    [HttpPost(ApiRoutes.Customer.AddAddress)]
    public async Task<IActionResult> AddAddress(
        Guid id,
        [FromBody] AddAddressRequest request)
    {
        var command = _mapper.Map<(Guid, AddAddressRequest), AddAddressCommand>(
            (id, request));

        var addAddressResult = await _mediator.Send(command);

        return addAddressResult.Match(
            result => Ok(_mapper.Map<Address, AddressResponse>(result)),
            Problem);
    }

    [HttpPut(ApiRoutes.Customer.UpdateAddress)]
    public async Task<IActionResult> UpdateAddress(
        Guid id,
        Guid addressId,
        [FromBody] UpdateAddressRequest request)
    {
        var command = _mapper.Map<(Guid, Guid, UpdateAddressRequest), UpdateAddressCommand>(
            (id, addressId, request));

        var updateAddressResult = await _mediator.Send(command);

        return updateAddressResult.Match(
            result => Ok(_mapper.Map<Address, AddressResponse>(result)),
            Problem);
    }
}
