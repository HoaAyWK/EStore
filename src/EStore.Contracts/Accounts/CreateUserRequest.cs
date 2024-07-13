namespace EStore.Contracts.Accounts;

public record CreateUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Role,
    bool IsEmailConfirmed,
    string? AvatarUrl);
