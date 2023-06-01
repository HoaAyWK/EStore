using ErrorOr;
using EStore.Domain.Catalog.CategoryAggregate;
using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    CategoryId? ParentId) : IRequest<ErrorOr<Category>>;
