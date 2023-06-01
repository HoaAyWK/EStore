using EStore.Domain.Entities;


namespace EStore.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token);

