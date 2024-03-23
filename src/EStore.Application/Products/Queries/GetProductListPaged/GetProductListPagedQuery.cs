using ErrorOr;
using EStore.Application.Products.Dtos;
using EStore.Contracts.Common;
using MediatR;

namespace EStore.Application.Products.Queries.GetProductListPaged;

public record GetProductListPagedQuery(
    string? SearchTerm,
    int Page,
    int PageSize,
    string? Order,
    string? OrderBy)
    : IRequest<PagedList<ProductDto>>;
