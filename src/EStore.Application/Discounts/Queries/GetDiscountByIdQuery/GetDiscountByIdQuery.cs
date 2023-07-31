using ErrorOr;
using EStore.Contracts.Discounts;
using EStore.Domain.DiscountAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Discounts.Queries.GetDiscountByIdQuery ;

public record GetDiscountByIdQuery(DiscountId DiscountId) : IRequest<ErrorOr<DiscountResponse>>;
