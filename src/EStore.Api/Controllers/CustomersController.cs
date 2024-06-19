using EStore.Api.Common.ApiRoutes;
using EStore.Api.Common.Contexts;
using EStore.Application.Customers.Command.UpdateCustomer;
using EStore.Application.Customers.Commands.AddAddress;
using EStore.Application.Customers.Commands.UpdateAddress;
using EStore.Application.Customers.Commands.DeleteAddress;
using EStore.Application.Customers.Queries.GetAllCustomers;
using EStore.Application.Customers.Queries.GetCustomerById;
using EStore.Application.Customers.Queries.GetCustomerStats;
using EStore.Contracts.Common;
using EStore.Contracts.Customers;
using EStore.Domain.CustomerAggregate.Entities;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Infrastructure.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class CustomersController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly IWorkContext _workContext;

    public CustomersController(
        ISender mediator,
        IMapper mapper,
        IWorkContext workContext)
    {
        _mediator = mediator;
        _mapper = mapper;
        _workContext = workContext;
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

    [HttpDelete(ApiRoutes.Customer.DeleteAddress)]
    public async Task<IActionResult> DeleteAddress(Guid id, Guid addressId)
    {
        var command = new DeleteAddressCommand(
            CustomerId.Create(id),
            AddressId.Create(addressId));

        var deleteAddressResult = await _mediator.Send(command);

        return deleteAddressResult.Match(
            _ => Ok(new { Id = addressId }),
            Problem);
    }

    [HttpGet(ApiRoutes.Customer.GetStats)]
    public async Task<IActionResult> GetStats([FromQuery] GetCustomerStatsRequest request)
    {
        var query = _mapper.Map<GetCustomerStatsRequest, GetCustomerStatsQuery>(request);
        var customerStats = await _mediator.Send(query);

        return Ok(customerStats);
    }

    [HttpGet(ApiRoutes.Customer.All)]
    [Authorize(Roles = Roles.Admin)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllCustomers()
    {
        var query = _mapper.Map<Guid, GetAllCustomersQuery>(_workContext.CustomerId);
        var customers = await _mediator.Send(query);

        return Ok(new { Data = customers});
    }
}
