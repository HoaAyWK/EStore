using ErrorOr;
using EStore.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Users.Command.UpdateUser;

public record UpdateUserCommand(
    UserId Id,
    string FirstName,
    string LastName)
    : IRequest<ErrorOr<Updated>>;
