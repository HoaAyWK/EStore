using ErrorOr;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(CategoryId Id) : IRequest<ErrorOr<Category?>>;
