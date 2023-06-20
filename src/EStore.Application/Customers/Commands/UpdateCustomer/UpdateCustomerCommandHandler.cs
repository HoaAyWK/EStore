using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Errors;
using EStore.Domain.CustomerAggregate.Repositories;
using MediatR;

namespace EStore.Application.Customers.Command.UpdateCustomer;

public class UpdateCustomerCommandHandler
    : IRequestHandler<UpdateCustomerCommand, ErrorOr<Updated>>
{
    private readonly ICustomerRepository _CustomerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerCommandHandler(
        ICustomerRepository CustomerRepository,
        IUnitOfWork unitOfWork)
    {
        _CustomerRepository = CustomerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _CustomerRepository.GetByIdAsync(request.Id);

        if (customer is null)
        {
            return Errors.Customer.NotFound;
        }

        var updateCustomerDetailResult = customer.UpdateDetails(request.FirstName, request.LastName);

        if (updateCustomerDetailResult.IsError)
        {
            return updateCustomerDetailResult.Errors;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
