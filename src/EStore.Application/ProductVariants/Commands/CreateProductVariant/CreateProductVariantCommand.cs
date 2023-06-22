using ErrorOr;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate;
using MediatR;

namespace EStore.Application.ProductVariants.Commands.CreateProductVariant;

public record CreateProductVariantCommand(
    ProductId ProductId,
    int StockQuantity,
    List<SelectedAttribute> SelectedAttributes,
    decimal? Price,
    bool IsActive,
    string? AssignedProductImageIds)
    : IRequest<ErrorOr<ProductVariant>>;
    
public record SelectedAttribute(
    ProductAttributeId ProductAttributeId,
    ProductAttributeValueId ProductAttributeValueId);
