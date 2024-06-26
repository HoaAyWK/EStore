using ErrorOr;
using EStore.Domain.BrandAggregate;
using MediatR;

namespace EStore.Application.Brands.Commands.CreateBrand;

public record CreateBrandCommand(
    string Name,
    string? ImageUrl)
    : IRequest<ErrorOr<Brand>>;
