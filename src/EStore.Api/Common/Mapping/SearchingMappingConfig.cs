using EStore.Application.Searching.Commands;
using EStore.Application.Searching.Queries.SearchProductsQuery;
using EStore.Contracts.Searching;
using EStore.Domain.ProductAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class SearchingMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SearchProductsRequest, SearchProductsQuery>()
            .Map(dest => dest.SearchQuery, src => src.Query);

        config.NewConfig<RebuildProductVariantRequest, RebuildProductVariantCommand>()
            .Map(dest => dest.ProductId, src => ProductId.Create(src.ProductId))
            .Map(dest => dest.ProductVariantId, src => ProductVariantId.Create(src.ProductVariantId));
    }
}
