using EStore.Application.Categories.Commands.CreateCategory;
using EStore.Contracts.Categories;
using EStore.Domain.Catalog.CategoryAggregate;
using EStore.Domain.Catalog.CategoryAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class CategoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCategoryRequest, CreateCategoryCommand>()
            .Map(
                dest => dest.ParentId,
                src => src.ParentId != null ? CategoryId.Create(src.ParentId.Value) : null);

        config.NewConfig<Category, CategoryResponse>()
            .Map(
                dest => dest.Id,
                src => src.Id.Value);
    }
}