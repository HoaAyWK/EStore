using EStore.Domain.BrandAggregate.ValueObjects;

namespace EStore.Domain.BrandAggregate.Repositories;

public interface IBrandReadRepository
{
    Task<List<Brand>> GetAllAsync();
    Task<Brand?> GetByIdAsync(BrandId brandId);
}
