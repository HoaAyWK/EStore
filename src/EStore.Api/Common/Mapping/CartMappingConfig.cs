using EStore.Application.Carts.Commands.AddItemToCart;
using EStore.Application.Carts.Commands.CreateCart;
using EStore.Application.Carts.Commands.RemoveItemFromCart;
using EStore.Application.Carts.Queries.GetCartByCustomerId;
using EStore.Contracts.Carts;
using EStore.Domain.CartAggregate.ValueObjects;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using EStore.Domain.ProductVariantAggregate.ValueObjects;
using Mapster;

namespace EStore.Api.Common.Mapping;

public class CartMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Guid, GetCartByCustomerIdQuery>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src));

        config.NewConfig<Guid, CreateCartCommand>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src));

        config.NewConfig<(Guid, AddItemToCartRequest), AddItemToCartCommand>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src.Item1))
            .Map(dest => dest.ProductId, src => ProductId.Create(src.Item2.ProductId))
            .Map(
                dest => dest.ProductVariantId,
                src => src.Item2.ProductVariantId != null
                    ? ProductVariantId.Create(src.Item2.ProductVariantId.Value)
                    : null);

        config.NewConfig<RemoveCartItemRequest, RemoveItemFromCartCommand>()
            .Map(dest => dest.CartId, src => CartId.Create(src.Id))
            .Map(dest => dest.ItemId, src => CartItemId.Create(src.ItemId));
    }
}
