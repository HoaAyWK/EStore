using EStore.Domain.BrandAggregate.ValueObjects;

namespace EStore.Domain.BrandAggregate.Repositories;

public interface IBrandRepository
{
    Task AddAsync(Brand brand);
    Task<Brand?> GetByIdAsync(BrandId id);
    Task<Brand?> GetByNameAsync(string name);
    void Delete(Brand brand);
}
