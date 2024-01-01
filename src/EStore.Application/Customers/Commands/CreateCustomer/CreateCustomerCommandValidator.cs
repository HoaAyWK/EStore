using EStore.Domain.CustomerAggregate;
using FluentValidation;

namespace EStore.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator
    : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

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
