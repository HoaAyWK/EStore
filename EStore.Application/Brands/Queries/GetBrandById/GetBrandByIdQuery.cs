using ErrorOr;
using EStore.Domain.BrandAggregate;
using EStore.Domain.BrandAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Brands.Queries.GetBrandById;

public record GetBrandByIdQuery(BrandId BrandId)
    : IRequest<ErrorOr<Brand>>;
