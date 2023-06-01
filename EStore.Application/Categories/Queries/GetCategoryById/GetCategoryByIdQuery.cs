using ErrorOr;
using EStore.Domain.Catalog.CategoryAggregate;
using MediatR;

namespace EStore.Application.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id) : IRequest<ErrorOr<Category?>>;
