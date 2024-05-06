using EStore.Contracts.Orders;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrdersByCriteria;

public record GetOrdersByCriteriaQuery(
    CustomerId CustomerId,
    ProductId ProductId,
    ProductVariantId? ProductVariantId)
    : IRequest<List<OrderResponse>>;
