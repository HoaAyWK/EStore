using EStore.Domain.BannerAggregate.ValueObjects;

namespace EStore.Domain.BannerAggregate.Repositories;

public interface IBannerRepository
{
    Task AddAsync(Banner banner, CancellationToken cancellationToken = default);
    Task<Banner?> GetByIdAsync(BannerId bannerId);
}
