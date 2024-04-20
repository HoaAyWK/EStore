using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.CustomerAggregate.Repositories;
using MediatR;

namespace EStore.Application.Customers.Command.UpdateCustomer;

public class UpdateCustomerCommandHandler
    : IRequestHandler<UpdateCustomerCommand, ErrorOr<Updated>>
{
    private readonly ICustomerRepository _CustomerRepository;

    public UpdateCustomerCommandHandler(ICustomerRepository CustomerRepository)
    {
        _CustomerRepository = CustomerRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var existingCustomerWithProvidedPhoneNumber = await _CustomerRepository
            .GetByPhoneNumberAsync(request.PhoneNumber);

        var customer = await _CustomerRepository.GetByIdAsync(request.Id);

        if (customer is null)
        {
            return Errors.Customer.NotFound;
        }

        if (existingCustomerWithProvidedPhoneNumber is not null &&
            existingCustomerWithProvidedPhoneNumber.Id != customer.Id)
        {
            return Errors.Customer.PhoneNumberAlreadyExists;
        }

        var updateCustomerDetailResult = customer.UpdateDetails(
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.AvatarUrl);

        if (updateCustomerDetailResult.IsError)
        {
            return updateCustomerDetailResult.Errors;
        }

        return Result.Updated;
    }
}
