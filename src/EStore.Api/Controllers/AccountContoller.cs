using EStore.Api.Common.ApiRoutes;
using EStore.Api.Common.Contexts;
using EStore.Application.Accounts.GetUserInfoQuery;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Customers.Commands.CreateCustomer;
using EStore.Contracts.Accounts;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AccountController : ApiController
{
    private readonly IWorkContext _workContext;
    private readonly ISender _mediator;
    private readonly IAccountService _accountService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IMapper _mapper;
    
    public AccountController(
        IWorkContext workContext,
        ISender mediator,
        IAccountService accountService,
        IAuthenticationService authenticationService,
        IMapper mapper)
    {
        _workContext = workContext;
        _mediator = mediator;
        _accountService = accountService;
        _authenticationService = authenticationService;
        _mapper = mapper;
    }

    [HttpGet(ApiRoutes.Account.MyProfile)]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = _workContext.CustomerId;
        var getUserInfoResult = await _mediator.Send(
            new GetUserInfoQuery(CustomerId.Create(userId)));

        return getUserInfoResult.Match(Ok, Problem);
    }

    [HttpPut(ApiRoutes.Account.ChangePassword)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = _workContext.CustomerId;
        var changePasswordResult = await _accountService.ChangePasswordAsync(
            CustomerId.Create(userId),
            request.OldPassword,
            request.NewPassword);

        return changePasswordResult.Match(result => Ok(new { Message = "Changed password" }), Problem);
    }

    [HttpPost(ApiRoutes.Account.CreateUser)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var command = _mapper.Map<CreateCustomerCommand>(request);
        var createUserResult = await _mediator.Send(command);

        if (createUserResult.IsError)
        {
            return Problem(createUserResult.Errors);
        }

        var user = createUserResult.Value;
        var createAppUserResult = await _authenticationService.CreateUserAsync(
            user,
            request.Password,
            request.Role,
            request.IsEmailConfirmed);

        if (createAppUserResult.IsError)
        {
            return Problem(createAppUserResult.Errors);
        }

        return Ok(new { CustomerId = user.Id });
    }
}
