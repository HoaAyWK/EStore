using ErrorOr;
using EStore.Application.Authentication.Common;
using MediatR;

namespace EStore.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<ErrorOr<AuthenticationResult>>;
