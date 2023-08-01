using ErrorOr;
using EStore.Domain.DiscountAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Discounts.Commands.UpdateDiscount;

public record UpdateDiscountCommand(
    DiscountId Id,
    string Name,
    bool UsePercentage,
    decimal DiscountPercentage,
    decimal DiscountAmount,
    DateTime StartDate,
    DateTime EndDate) : IRequest<ErrorOr<Updated>>;
