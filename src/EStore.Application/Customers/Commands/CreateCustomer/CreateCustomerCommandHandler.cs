using ErrorOr;
using EStore.Domain.Common.Errors;
using EStore.Domain.CustomerAggregate;
using EStore.Domain.CustomerAggregate.Repositories;
using MediatR;

namespace EStore.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler
    : IRequestHandler<CreateCustomerCommand, ErrorOr<Customer>>
{
    private readonly ICustomerRepository _CustomerRepository;

    public CreateCustomerCommandHandler(ICustomerRepository CustomerRepository)
    {
        _CustomerRepository = CustomerRepository;
    }

    public async Task<ErrorOr<Customer>> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var createCustomerResult = Customer.Create(
            request.Email,
            request.FirstName,
            request.LastName,
            request.PhoneNumber);

        if (createCustomerResult.IsError)
        {
            return createCustomerResult.Errors;
        }

        var CustomerWithEmailExisted = await _CustomerRepository.GetByEmailAsync(request.Email);

        if (CustomerWithEmailExisted is not null)
        {
            return Errors.Customer.DuplicateEmail;
        }

        var customer = createCustomerResult.Value;

        if (request.AvatarUrl is not null)
        {
            var updateCustomerResult = customer.UpdateDetails(
                customer.FirstName,
                customer.LastName,
                customer.PhoneNumber!,
                request.AvatarUrl);

            if (updateCustomerResult.IsError)
            {
                return updateCustomerResult.Errors;
            }
        }

        await _CustomerRepository.AddAsync(customer);

        return customer;
    }
}

