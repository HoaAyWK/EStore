using ErrorOr;
using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(CategoryId Id)
    : IRequest<ErrorOr<Deleted>>;
