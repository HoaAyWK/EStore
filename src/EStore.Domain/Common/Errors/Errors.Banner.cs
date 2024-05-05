using ErrorOr;
using EStore.Domain.BannerAggregate.ValueObjects;

namespace EStore.Domain.Common.Errors;

public static partial class Errors
{
    public static class Banner
    {
        public static Error NotFound = Error.NotFound(
            code: "Banner.NotFound",
            description: "Banner not found.");
    }
}
