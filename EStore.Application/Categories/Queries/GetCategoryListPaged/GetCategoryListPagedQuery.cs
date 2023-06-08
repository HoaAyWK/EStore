using EStore.Domain.CategoryAggregate;
using MediatR;

namespace EStore.Application.Categories.Queries.GetCategoryListPaged;

public record GetCategoryListPagedQuery()
    : IRequest<List<Category>>;
