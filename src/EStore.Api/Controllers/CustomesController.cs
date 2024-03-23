using EStore.Api.Common.ApiRoutes;
using EStore.Application.Customers.Command.UpdateCustomer;
using EStore.Application.Customers.Queries;
using EStore.Contracts.Customers;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class CustomersController : ApiController
{
    private readonly ISender _mediator;

    public CustomersController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPut(ApiRoutes.Customer.Update)]
    public async Task<IActionResult> UpdateCustomer(
        Guid id,
        [FromBody] UpdateCustomerRequest request)
    {

        var command = new UpdateCustomerCommand(
            CustomerId.Create(id),
            request.FirstName,
            request.LastName);

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
}
