namespace EStore.Contracts.Accounts;

public record UserInfoResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    string? AvatarUrl);
