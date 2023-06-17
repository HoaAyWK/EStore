using EStore.Domain.CategoryAggregate;
using MediatR;

namespace EStore.Application.Categories.Queries.GetAllParentsWithChildren;

public record GetAllParentsWithChildrenQuery()
    : IRequest<List<Category>>;