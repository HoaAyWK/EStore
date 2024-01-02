using EStore.Domain.Common.Models;
using EStore.Domain.Common.Specifications;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Specifications;

public static class SpecificationEvaluator<TId>
    where TId : notnull
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQueryable,
        Specification<TEntity, TId> specification)
        where TEntity : AggregateRoot<TId>
    {
        IQueryable<TEntity> queryable = inputQueryable;

        if (specification.IsAsNoTrackingEnabled)
        {
            queryable = queryable.AsNoTracking();
        }

        if (specification.Criteria is not null)
        {
            queryable = queryable.Where(specification.Criteria);
        }

        queryable = specification.IncludeExpressions.Aggregate(
            queryable,
            (current, includeExpression)
                => current.Include(includeExpression));

        queryable = specification.IncludeStrings.Aggregate(
            queryable,
            (current, includeString)
                => current.Include(includeString));

        if (specification.OrderByExpression is not null)
        {
            queryable = queryable.OrderBy(specification.OrderByExpression);
        }
        else if (specification.OrderByDescendingExpression is not null)
        {
            queryable = queryable.OrderByDescending(specification.OrderByDescendingExpression);
        }

        if (specification.IsPagingEnabled)
        {
            queryable = queryable.Skip(specification.Skip)
                .Take(specification.Take);
        }

        return queryable;
    }
}
