namespace EStore.Contracts.Accounts;

public record ChangePasswordRequest(
    string OldPassword,
    string NewPassword);
