using ErrorOr;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    string Slug,
    string? ImageUrl,
    CategoryId? ParentId) : IRequest<ErrorOr<Category>>;
