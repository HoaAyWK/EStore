namespace EStore.Contracts.Authentication;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password);

