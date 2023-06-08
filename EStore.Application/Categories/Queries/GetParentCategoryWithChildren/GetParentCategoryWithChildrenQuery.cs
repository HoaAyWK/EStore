using EStore.Domain.CategoryAggregate;
using MediatR;

namespace EStore.Application.Categories.Queries.GetParentCategoryWithChildren;

public record GetParentCategoryWithChildrenQuery()
    : IRequest<List<Category>>;