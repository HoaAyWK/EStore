using ErrorOr;
using EStore.Application.Authentication.Common;
using MediatR;

namespace EStore.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
