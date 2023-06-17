using EStore.Contracts.Categories;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.Common.Collections;
using MediatR;

namespace EStore.Application.Categories.Queries.GetCategoryTree;

public record GetCategoryTreeQuery
    : IRequest<List<TreeNode<CategoryWithPathResponse>>>;
