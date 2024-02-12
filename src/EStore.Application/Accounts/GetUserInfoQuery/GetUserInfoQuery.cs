using ErrorOr;
using EStore.Contracts.Accounts;
using MediatR;

namespace EStore.Application.Accounts.GetUserInfoQuery;

public record GetUserInfoQuery(Guid UserId)
    : IRequest<ErrorOr<UserInfoResponse>>;
