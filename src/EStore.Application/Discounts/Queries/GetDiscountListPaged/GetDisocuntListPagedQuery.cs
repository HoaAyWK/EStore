using EStore.Contracts.Discount;
using MediatR;

namespace EStore.Application.Discounts.Queries.GetDiscountListPaged;

public record GetDiscountListPagedQuery(int PageSize, int Page)
    : IRequest<ListPagedDiscountResponse>;
