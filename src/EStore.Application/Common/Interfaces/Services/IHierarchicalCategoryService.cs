using EStore.Domain.CategoryAggregate;

namespace EStore.Application.Common.Interfaces.Services;

public interface IHierarchicalCategoryService
{
    dynamic GetHierarchy(Category category, string separator = " > ");
}
