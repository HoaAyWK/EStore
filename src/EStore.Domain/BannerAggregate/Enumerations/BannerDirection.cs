using EStore.Domain.Common.Abstractions;

namespace EStore.Domain.BannerAggregate.Enumerations;

public sealed class BannerDirection : Enumeration<BannerDirection>
{
    public static readonly BannerDirection Horizontal = new(1, nameof(Horizontal));

    public static readonly BannerDirection Vertical = new(2, nameof(Vertical));

    private BannerDirection(int value, string name)
        : base(value, name)
    {
    }
}