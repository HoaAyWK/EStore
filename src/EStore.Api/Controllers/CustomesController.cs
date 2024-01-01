using EStore.Api.Common.ApiRoutes;
using EStore.Application.Customers.Command.UpdateCustomer;
using EStore.Contracts.Customers;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MapsterMapper;
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
            errors => Problem(errors));
    }
}
