using EStore.Contracts.Categories;
using EStore.Domain.CategoryAggregate;
using MediatR;

namespace EStore.Application.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQuery()
    : IRequest<List<CategoryWithPathResponse>>;
