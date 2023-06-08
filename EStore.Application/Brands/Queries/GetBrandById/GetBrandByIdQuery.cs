using ErrorOr;
using EStore.Domain.Catalog.BrandAggregate;
using EStore.Domain.Catalog.BrandAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Brands.Queries.GetBrandById;

public record GetBrandByIdQuery(BrandId BrandId)
    : IRequest<ErrorOr<Brand>>;
