using EStore.Api.Common.ApiRoutes;
using EStore.Api.Common.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Api.Controllers;

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

        var getUserInfoResult = await _mediator.Send(new GetUserInfoQuery(userId));
    }
}
