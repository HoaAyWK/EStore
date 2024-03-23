using EStore.Api.Common.ApiRoutes;
using EStore.Api.Common.Contexts;
using EStore.Application.Accounts.GetUserInfoQuery;
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
    
    public AccountController(IWorkContext workContext, ISender mediator)
    {
        _workContext = workContext;
        _mediator = mediator;
    }

    [HttpGet(ApiRoutes.Account.MyProfile)]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = _workContext.CustomerId;
        var getUserInfoResult = await _mediator.Send(
            new GetUserInfoQuery(CustomerId.Create(userId)));

        return getUserInfoResult.Match(Ok, Problem);
    }
}
