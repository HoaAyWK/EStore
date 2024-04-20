using ErrorOr;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class General
    {
        public static Error UnProcessableRequest => Error.Failure(
            code: "General.UnProcessableRequest",
            description: "The server could not process the request.");
    }
}
