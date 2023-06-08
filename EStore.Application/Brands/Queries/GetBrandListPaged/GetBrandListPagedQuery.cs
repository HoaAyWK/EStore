using EStore.Domain.BrandAggregate;
using MediatR;

namespace EStore.Application.Brands.Queries.GetBrandListPaged;

public record GetBrandListPagedQuery()
    : IRequest<List<Brand>>;
