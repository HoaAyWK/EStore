using EStore.Contracts.Brands;
using MediatR;

namespace EStore.Application.Brands.Queries.GetBrandListPaged;

public record GetBrandListPagedQuery(int PageSize, int Page)
    : IRequest<ListPagedBrandResponse>;
