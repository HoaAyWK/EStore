using ErrorOr;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Notification
    {
        public static Error NotFound = Error.NotFound(
            code: "Notification.NotFound",
            description: "Notification not found.");

    }
}
