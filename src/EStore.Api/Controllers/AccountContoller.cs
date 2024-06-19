using EStore.Api.Common.ApiRoutes;
using EStore.Api.Common.Contexts;
using EStore.Application.Accounts.GetUserInfoQuery;
using EStore.Application.Common.Interfaces.Services;
using EStore.Contracts.Accounts;
using EStore.Domain.CustomerAggregate.ValueObjects;
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
    
    public AccountController(
        IWorkContext workContext,
        ISender mediator,
        IAccountService accountService)
    {
        _workContext = workContext;
        _mediator = mediator;
        _accountService = accountService;
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
}
