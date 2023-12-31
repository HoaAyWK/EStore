using ErrorOr;
using EStore.Domain.BrandAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Brands.Commands.DeleteBrand;

public record DeleteBrandCommand(BrandId Id)
    : IRequest<ErrorOr<Deleted>>;
