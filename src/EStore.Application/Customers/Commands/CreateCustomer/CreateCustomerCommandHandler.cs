using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Errors;
using EStore.Domain.CustomerAggregate;
using EStore.Domain.CustomerAggregate.Repositories;
using MediatR;

namespace EStore.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler
    : IRequestHandler<CreateCustomerCommand, ErrorOr<Customer>>
{
    private readonly ICustomerRepository _CustomerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(
        ICustomerRepository CustomerRepository,
        IUnitOfWork unitOfWork)
    {
        _CustomerRepository = CustomerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Customer>> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var createCustomerResult = Customer.Create(request.Email, request.FirstName, request.LastName);

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

        await _CustomerRepository.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return customer;
    }
}

