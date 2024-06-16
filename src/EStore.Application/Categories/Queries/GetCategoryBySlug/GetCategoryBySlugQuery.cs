using ErrorOr;
using EStore.Contracts.Categories;
using MediatR;

namespace EStore.Application.Categories.Queries.GetCategoryBySlug;

public record GetCategoryBySlugQuery(string Slug)
    : IRequest<ErrorOr<CategoryWithPathsResponse>>;
