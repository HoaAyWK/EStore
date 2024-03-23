using ErrorOr;
using EStore.Contracts.Accounts;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Accounts.GetUserInfoQuery;

public record GetUserInfoQuery(CustomerId UserId)
    : IRequest<ErrorOr<UserInfoResponse>>;
