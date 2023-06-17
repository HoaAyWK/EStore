using ErrorOr;
using EStore.Domain.CategoryAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(
    CategoryId Id,
    string Name,
    CategoryId? ParentId)
    : IRequest<ErrorOr<Updated>>;
