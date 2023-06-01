using Microsoft.AspNetCore.Mvc;
using EStore.Contracts.Authentication;
using MediatR;
using EStore.Application.Authentication.Commands.Register;
using EStore.Application.Authentication.Queries.Login;
using MapsterMapper;

namespace EStore.Api.Controllers;

[Route("auth")]
public class AuthController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        var authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);
        var authResult = await _mediator.Send(query);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors)
        );
    }
}
