using ErrorOr;
using EStore.Domain.UserAggregate;
using MediatR;

namespace EStore.Application.Users.Command.CreateUser;

public record CreateUserCommand(
    string Email,
    string FirstName,
    string LastName)
    : IRequest<ErrorOr<User>>;
