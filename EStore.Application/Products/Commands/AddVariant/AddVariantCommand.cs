using ErrorOr;
using EStore.Domain.ProductAggregate;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.AddVariant;

public record AddVariantCommand(
    ProductId ProductId,
    List<AttributeSelection> AttributeSelections,
    decimal Price,
    int StockQuantity,
    bool? IsActive,
    List<Guid> AssignedImageIds)
    : IRequest<ErrorOr<Product>>;

public record AttributeSelection(
    ProductAttributeId ProductAttributeId,
    ProductAttributeValueId ProductAttributeValueId);