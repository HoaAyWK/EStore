using EStore.Application.Orders.Commands.RefundOrder;
using EStore.Application.Orders.Queries.GetOrderListPaged;
using EStore.Contracts.Common;
using EStore.Contracts.Orders;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Entities;
using EStore.Domain.OrderAggregate.ValueObjects;
using Mapster;
using static EStore.Contracts.Orders.OrderResponse;

namespace EStore.Api.Common.Mapping;

public class OrderMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Order, OrderResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.OrderStatus, src => src.OrderStatus.Name)
            .Map(dest => dest.CustomerId, src => src.CustomerId.Value)
            .Map(dest => dest.ShippingAddress, src => src.ShippingAddress)
            .Map(dest => dest.OrderItems, src => src.OrderItems)
            .Map(dest => dest.TotalAmount, src => src.TotalAmount);

        config.NewConfig<OrderItem, OrderItemResponse>()
            .Map(dest => dest.ProductId, src => src.ItemOrdered.ProductId.Value)
            .Map(dest => dest.ProductVariantId, src => src.ItemOrdered.ProductVariantId!.Value)
            .Map(dest => dest.ProductName, src => src.ItemOrdered.ProductName )
            .Map(dest => dest.ProductAttributes, src => src.ItemOrdered.ProductAttributes)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Map(dest => dest.SubTotal, src => src.SubTotal)
            .Map(dest => dest.Quantity, src => src.Quantity);

        config.NewConfig<ShippingAddress, ShippingAddressResponse>();

        config.NewConfig<Guid, OrderId>()
            .Map(dest => dest, src => OrderId.Create(src));

        config.NewConfig<Guid, RefundOrderCommand>()
            .Map(dest => dest.OrderId, src => OrderId.Create(src));

        config.NewConfig<PagedList<Order>, PagedList<OrderResponse>>()
            .Map(dest => dest.Items, src => src.Items);

        config.NewConfig<(int, int), GetOrderListPagedQuery>()
            .Map(dest => dest.Page, src => src.Item1)
            .Map(dest => dest.PageSize, src => src.Item2);
    }
}
