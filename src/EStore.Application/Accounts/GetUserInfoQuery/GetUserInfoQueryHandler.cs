using ErrorOr;
using EStore.Application.Common.Interfaces.Services;
using EStore.Contracts.Accounts;
using MediatR;

namespace EStore.Application.Accounts.GetUserInfoQuery;

public class GetUserInfoQueryHandler
    : IRequestHandler<GetUserInfoQuery, ErrorOr<UserInfoResponse>>
{
    private readonly IAccountService _accountService;

    public GetUserInfoQueryHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<ErrorOr<UserInfoResponse>> Handle(
        GetUserInfoQuery request,
        CancellationToken cancellationToken)
    {
        return await _accountService.GetUserInfoAsync(request.UserId);
    }
}
