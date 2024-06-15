using EStore.Application.Discounts.Commands.UpdateDiscount;
using EStore.Application.Discounts.Queries.GetDiscountByIdQuery;
using EStore.Application.Products.Commands.AssignDiscount;
using EStore.Contracts.Discounts;
using EStore.Domain.DiscountAggregate;
using EStore.Domain.DiscountAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class DiscountMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Guid, GetDiscountByIdQuery>()
            .Map(dest => dest.DiscountId, src => DiscountId.Create(src));

        config.NewConfig<Discount, DiscountResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<(Guid, UpdateDiscountRequest), UpdateDiscountCommand>()
            .Map(dest => dest.Id, src => DiscountId.Create(src.Item1))
            .Map(dest => dest, src => src.Item2);

        config.NewConfig<(Guid, AssignDiscountRequest), AssignDiscountCommand>()
            .Map(dest => dest.ProductId, src => ProductId.Create(src.Item1))
            .Map(
                dest => dest.DiscountId,
                src => src.Item2.DiscountId == null
                    ? null
                    : DiscountId.Create(src.Item2.DiscountId.Value));
    }
}
