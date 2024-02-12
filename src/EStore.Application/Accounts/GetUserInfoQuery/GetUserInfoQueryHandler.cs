using ErrorOr;
using EStore.Contracts.Accounts;
using EStore.Domain.Common.Errors;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Accounts.GetUserInfoQuery;

public class GetUserInfoQueryHandler
    : IRequestHandler<GetUserInfoQuery, ErrorOr<UserInfoResponse>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetUserInfoQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ErrorOr<UserInfoResponse>> Handle(
        GetUserInfoQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _customerRepository.GetByIdAsync(
            CustomerId.Create(request.UserId));

        if (user is null)
        {
            return Errors.Account.NotFound;
        }

        return new UserInfoResponse(
            user.Id.Value,
            user.FirstName,
            user.LastName,
            user.Email,
            null);
    }
}
