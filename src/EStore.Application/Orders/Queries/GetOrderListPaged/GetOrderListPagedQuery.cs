using EStore.Contracts.Common;
using EStore.Domain.OrderAggregate;
using MediatR;

namespace EStore.Application.Orders.Queries.GetOrderListPaged;

public record GetOrderListPagedQuery(int Page, int PageSize) : IRequest<PagedList<Order>>;
