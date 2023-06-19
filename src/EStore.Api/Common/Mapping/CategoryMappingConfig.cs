using EStore.Application.Categories.Commands.CreateCategory;
using EStore.Application.Categories.Commands.UpdateCategory;
using EStore.Application.Common.Dtos;
using EStore.Contracts.Categories;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.CategoryAggregate.ValueObjects;
using EStore.Domain.Common.Collections;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class CategoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid, UpdateCategoryRequest), UpdateCategoryCommand>()
            .Map(dest => dest.Id, src => CategoryId.Create(src.Item1))
            .Map(
                dest => dest.ParentId,
                src => src.Item2.ParentId != null
                    ? CategoryId.Create(new Guid(src.Item2.ParentId))
                    : null)
            .Map(dest => dest.Name, src => src.Item2.Name);

        config.NewConfig<Guid, CategoryId>()
            .Map(dest => dest, src => CategoryId.Create(src));

        config.NewConfig<CreateCategoryRequest, CreateCategoryCommand>()
            .Map(
                dest => dest.ParentId,
                src => src.ParentId != null
                    ? CategoryId.Create(new Guid(src.ParentId))
                    : null);

        config.NewConfig<Category, CategoryResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.ParentId, src => src.ParentId!.Value);

        config.NewConfig<Category, CategoryWithChildrenResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.ParentId, src => src.Parent!.Id.Value);

        config.NewConfig<ListPagedCategoryResult, ListPagedCategoryResponse>()
            .Map(dest => dest.Data, src => src.Categories);

        config.NewConfig<TreeNode<CategoryWithPathResponse>, CategoryNodeResponse>()
            .Map(dest => dest.Children, src => src.Children)
            .Map(dest => dest.Id, src => src.Data.Id)
            .Map(dest => dest.Level, src => src.Level)
            .Map(dest => dest.Name, src => src.Data.Name)
            .Map(dest => dest.ParentId, src => src.Data.ParentId);
    }
}