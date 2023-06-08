using ErrorOr;
using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(
    CategoryId Id,
    string Name,
    CategoryId? ParentId)
    : IRequest<ErrorOr<Updated>>;
