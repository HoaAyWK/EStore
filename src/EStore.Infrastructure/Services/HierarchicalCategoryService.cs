using System.Dynamic;
using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.CategoryAggregate;
using Microsoft.IdentityModel.Tokens;

namespace EStore.Infrastructure.Services;

public class HierarchicalCategoryService : IHierarchicalCategoryService
{
    public dynamic GetHierarchy(Category category, string separator = " > ")
    {
        var hierarchy = new ExpandoObject();
        var hierarchyCategories = new LinkedList<string>();

        var current = category;

        while (current is not null)
        {
            hierarchyCategories.AddFirst(current.Name);
            current = current.Parent;
        }

        var path = string.Empty;

        for (int i = 0; i < hierarchyCategories.Count; i++)
        {
            if (path.IsNullOrEmpty())
            {
                path = hierarchyCategories.ElementAt(i);
            }
            else
            {
                path += separator + hierarchyCategories.ElementAt(i);
            }
            
            ((IDictionary<string, object>)hierarchy!)["lvl" + i] = path;
        }

        return hierarchy;
    }
}
