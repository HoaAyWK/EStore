using EStore.Application.Users.Command.UpdateUser;
using EStore.Contracts.Users;
using EStore.Domain.UserAggregate.ValueObjects;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

public class UsersController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public UsersController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(
        Guid id,
        [FromBody] UpdateUserRequest request)
    {

        var command = new UpdateUserCommand(
            UserId.Create(id),
            request.FirstName,
            request.LastName);

        var updateUserResult = await _mediator.Send(command);

        return updateUserResult.Match(
            updated => NoContent(),
            errors => Problem(errors));
    }
}
