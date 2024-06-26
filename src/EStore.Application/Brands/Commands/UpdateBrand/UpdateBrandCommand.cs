using ErrorOr;
using EStore.Domain.BrandAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Brands.Commands.UpdateBrand;

public record UpdateBrandCommand(BrandId Id, string Name, string? ImageUrl)
    : IRequest<ErrorOr<Updated>>;
