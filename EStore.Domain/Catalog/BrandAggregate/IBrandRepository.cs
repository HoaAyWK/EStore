using EStore.Domain.Catalog.BrandAggregate.ValueObjects;

namespace EStore.Domain.Catalog.BrandAggregate;

public interface IBrandRepository
{
    Task AddAsync(Brand brand);
    Task<Brand?> GetByIdAsync(BrandId id);
}
