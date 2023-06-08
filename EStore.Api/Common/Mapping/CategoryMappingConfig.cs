using EStore.Application.Categories.Commands.CreateCategory;
using EStore.Application.Categories.Commands.UpdateCategory;
using EStore.Contracts.Categories;
using EStore.Domain.CategoryAggregate;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class CategoryMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UpdateCategoryRequest, UpdateCategoryCommand>();

        config.NewConfig<Category, CategoryResponse>()
            .Map(
                dest => dest.Id,
                src => src.Id.Value);
    }
}