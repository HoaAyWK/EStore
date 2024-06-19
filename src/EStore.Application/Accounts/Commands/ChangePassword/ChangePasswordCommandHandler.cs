using ErrorOr;
using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.Common.Errors;
using EStore.Domain.CustomerAggregate.Repositories;
using MediatR;

namespace EStore.Application.Accounts.Commands.ChangePassword;

public class ChangePasswordCommandHandler
    : IRequestHandler<ChangePasswordCommand, ErrorOr<Success>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IAccountService _accountService;

    public ChangePasswordCommandHandler(
        ICustomerRepository customerRepository,
        IAccountService accountService)
    {
        _customerRepository = customerRepository;
        _accountService = accountService;
    }

    public async Task<ErrorOr<Success>> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

        if (customer is null)
        {
            return Errors.Account.NotFound;
        }

        return await _accountService.ChangePasswordAsync(
            request.CustomerId,
            request.OldPassword,
            request.NewPassword);
    }
}
