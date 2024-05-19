using EStore.Contracts.Orders;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrderStats;

public record GetOrderStatsQuery(int LastDaysCount)
    : IRequest<OrderStats>;
