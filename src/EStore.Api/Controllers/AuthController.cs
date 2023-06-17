using Microsoft.AspNetCore.Mvc;
using EStore.Contracts.Authentication;
using MediatR;
using MapsterMapper;
using EStore.Application.Users.Command.CreateUser;
using EStore.Application.Common.Interfaces.Services;

namespace EStore.Api.Controllers;

public class AuthController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;

    public AuthController(
        ISender mediator,
        IMapper mapper,
        IAuthenticationService authenticationService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<CreateUserCommand>(request);
        var createUserResult = await _mediator.Send(command);

        if (createUserResult.IsError)
        {
            return Problem(createUserResult.Errors);
        }

        var user = createUserResult.Value;
        var authResult = await _authenticationService.RegisterAsync(user, request.Password);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var authResult = await _authenticationService.LoginAsync(request.Email, request.Password);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }
}
