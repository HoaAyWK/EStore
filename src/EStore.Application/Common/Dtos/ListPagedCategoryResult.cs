using EStore.Domain.CategoryAggregate;

namespace EStore.Application.Common.Dtos;

public record ListPagedCategoryResult(
    List<Category> Categories,
    int PageSize,
    int Page,
    int TotalItems);
