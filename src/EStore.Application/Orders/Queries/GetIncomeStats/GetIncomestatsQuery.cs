using EStore.Contracts.Orders;
using MediatR;

namespace EStore.Application.Orders.Queries.GetIncomeStats;

public record GetIncomeStatsQuery(int LastDaysCount) : IRequest<IncomeStats>;
