using EStore.Domain.OrderAggregate.Enumerations;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrderStatuses;

public record GetOrderStatusesQuery : IRequest<List<OrderStatus>>;
