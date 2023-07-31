using ErrorOr;
using EStore.Domain.DiscountAggregate;
using MediatR;

namespace EStore.Application.Discounts.Commands.CreateDiscount;

public record CreateDiscountCommand(
    string Name,
    bool UsePercentage,
    decimal DiscountPercentage,
    decimal DiscountAmount,
    DateTime StartDate,
    DateTime EndDate)
    : IRequest<ErrorOr<Discount>>;
