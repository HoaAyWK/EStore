namespace EStore.Contracts.Users;

public record UpdateUserRequest(
    string FirstName,
    string LastName,
    List<Guid>? RoleIds);
