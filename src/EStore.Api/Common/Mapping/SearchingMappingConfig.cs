using EStore.Application.Searching.Queries.SearchProductsQuery;
using EStore.Contracts.Searching;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class SearchingMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SearchProductsRequest, SearchProductsQuery>()
            .Map(dest => dest.SearchQuery, src => src.Query);
    }
}
