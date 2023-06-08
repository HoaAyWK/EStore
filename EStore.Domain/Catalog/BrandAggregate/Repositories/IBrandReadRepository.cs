using EStore.Domain.Catalog.BrandAggregate.ValueObjects;

namespace EStore.Domain.Catalog.BrandAggregate.Repositories;

public interface IBrandReadRepository
{
    Task<List<Brand>> GetAllAsync();
    Task<Brand?> GetByIdAsync(BrandId brandId);
}
