using EStore.Application.Customers.Command.UpdateCustomer;
using EStore.Domain.CustomerAggregate;
using FluentValidation;

namespace EStore.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator
    : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(Customer.MinFirstNameLength)
            .MaximumLength(Customer.MaxFirstNameLength);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(Customer.MinLastNameLength)
            .MaximumLength(Customer.MaxLastNameLength);
    }
}
