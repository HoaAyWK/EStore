using System.Linq.Expressions;
using EStore.Domain.Common.Models;

namespace EStore.Domain.Common.Specifications;

public interface ISpecification<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    Expression<Func<TEntity, bool>>? Criteria { get; }
    List<Expression<Func<TEntity, object>>> IncludeExpressions { get; }
    List<string> IncludeStrings { get; }
    Expression<Func<TEntity, object>>? OrderByExpression { get; }
    Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; }
    int Skip { get; }
    int Take { get; }
    bool IsPagingEnabled { get; }
    bool IsAsNoTrackingEnabled { get; }
}
