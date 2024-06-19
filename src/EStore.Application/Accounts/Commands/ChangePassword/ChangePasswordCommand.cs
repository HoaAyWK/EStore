using ErrorOr;
using EStore.Domain.CustomerAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Accounts.Commands.ChangePassword;

public record ChangePasswordCommand(
    CustomerId CustomerId,
    string OldPassword,
    string NewPassword)
    : IRequest<ErrorOr<Success>>;
