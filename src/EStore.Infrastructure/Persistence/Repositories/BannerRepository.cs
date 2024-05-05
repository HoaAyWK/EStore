using EStore.Domain.BannerAggregate;
using EStore.Domain.BannerAggregate.Repositories;
using EStore.Domain.BannerAggregate.ValueObjects;

namespace EStore.Infrastructure.Persistence.Repositories;

internal class BannerRepository : IBannerRepository
{
    private readonly EStoreDbContext _dbContext;

    public BannerRepository(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        Banner banner,
        CancellationToken cancellationToken = default)
        => await _dbContext.Banners.AddAsync(banner, cancellationToken);

    public async Task<Banner?> GetByIdAsync(BannerId bannerId)
        => await _dbContext.Banners.FindAsync(bannerId);
}
