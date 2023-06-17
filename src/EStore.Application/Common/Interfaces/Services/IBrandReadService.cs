using EStore.Contracts.Brands;
using EStore.Domain.BrandAggregate.ValueObjects;

namespace EStore.Application.Common.Interfaces.Services;

public interface IBrandReadService
{
    Task<BrandResponse?> GetByIdAsync(BrandId brandId);
    Task<ListPagedBrandResponse> GetListPagedAsync(int pageSize = 0, int page = 0);
}
