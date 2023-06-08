using EStore.Domain.Catalog.BrandAggregate.ValueObjects;

namespace EStore.Domain.Catalog.BrandAggregate.Repositories;

public interface IBrandRepository
{
    Task AddAsync(Brand brand);
    Task<Brand?> GetByIdAsync(BrandId id);
    void Delete(Brand brand);
}
