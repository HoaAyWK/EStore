using ErrorOr;
using EStore.Domain.DiscountAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Products.Commands.AssignDiscount;

public record AssignDiscountCommand(
    ProductId ProductId,
    DiscountId? DiscountId)
    : IRequest<ErrorOr<Success>>;
