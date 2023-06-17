using EStore.Application.Common.Dtos;
using MediatR;

namespace EStore.Application.Categories.Queries.GetCategoryListPaged;

public record GetCategoryListPagedQuery(int PageSize, int Page)
    : IRequest<ListPagedCategoryResult>;
