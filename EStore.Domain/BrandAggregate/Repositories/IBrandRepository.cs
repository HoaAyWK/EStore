using EStore.Domain.BrandAggregate.ValueObjects;

namespace EStore.Domain.BrandAggregate.Repositories;

public interface IBrandRepository
{
    Task AddAsync(Brand brand);
    Task<Brand?> GetByIdAsync(BrandId id);
    void Delete(Brand brand);
}
