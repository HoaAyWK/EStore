using EStore.Domain.BrandAggregate.ValueObjects;
using EStore.Domain.Common.Specifications;

namespace EStore.Domain.BrandAggregate.Specifications;

public class GetBrandByIdSpec : Specification<Brand, BrandId>
{
    public GetBrandByIdSpec(BrandId brandId)
        : base(brand => brand.Id == brandId)
    {
    }
}
