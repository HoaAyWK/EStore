namespace EStore.Contracts.Authentication;

public record VerifyEmailRequest(string Email, string Token);
