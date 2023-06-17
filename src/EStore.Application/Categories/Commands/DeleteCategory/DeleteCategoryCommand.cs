using ErrorOr;
using EStore.Domain.CategoryAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(CategoryId Id)
    : IRequest<ErrorOr<Deleted>>;
