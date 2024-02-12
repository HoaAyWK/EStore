namespace EStore.Contracts.Authentication;

public record ResetPasswordRequest(
    string Email,
    string Token,
    string Password);
