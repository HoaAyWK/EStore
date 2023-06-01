using System.Linq.Expressions;
using EStore.Domain.Common.Models;

namespace EStore.Domain.Common.Specifications;

public abstract class Specification<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    public Expression<Func<TEntity, bool>>? Criteria { get; }

    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }

    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }


    protected Specification(Expression<Func<TEntity, bool>>? criteria)
    {
        Criteria = criteria;
    }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        IncludeExpressions.Add(includeExpression);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderByExpression = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
    {
        OrderByDescendingExpression = orderByDescendingExpression;
    }
}