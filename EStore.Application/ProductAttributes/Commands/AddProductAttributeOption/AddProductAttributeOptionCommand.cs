using ErrorOr;
using EStore.Domain.Catalog.ProductAttributeAggregate;
using MediatR;

namespace EStore.Application.ProductAttributes.Commands.AddProductAttributeOption;

public record AddProductAttributeOptionCommand(
    string ProductAttributeId,
    string ProductAttributeOptionSetId,
    string Name,
    decimal? PriceAdjustment,
    string? Alias) : IRequest<ErrorOr<ProductAttributeOption>>;
