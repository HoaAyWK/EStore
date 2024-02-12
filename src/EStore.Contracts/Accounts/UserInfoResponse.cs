namespace EStore.Contracts.Accounts;

public record UserInfoResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? AvatarUrl);
