using EStore.Contracts.Searching;
using MediatR;

namespace EStore.Application.Searching.Queries.SearchProductsQuery;

public record SearchProductsQuery(
    string? SearchQuery,
    int Page,
    int PageSize)
    : IRequest<SearchProductListPagedResponse>;
