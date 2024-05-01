using EStore.Application.Orders.Commands.ConfirmPaymentInfo;
using EStore.Application.Orders.Commands.CreateOrder;
using EStore.Application.Orders.Commands.RefundOrder;
using EStore.Application.Orders.Queries.GetOrderById;
using EStore.Application.Orders.Queries.GetOrderListPaged;
using EStore.Application.Orders.Queries.GetOrdersByCustomer;
using EStore.Contracts.Common;
using EStore.Contracts.Orders;
using EStore.Domain.CustomerAggregate.ValueObjects;
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
            .Map(dest => dest.OrderNumber, src => src.OrderNumber)
            .Map(dest => dest.OrderStatus, src => src.OrderStatus.Name)
            .Map(dest => dest.PaymentMethod, src => src.PaymentMethod.Name)
            .Map(dest => dest.PaymentStatus, src => src.PaymentStatus.Name)
            .Map(dest => dest.CustomerId, src => src.CustomerId.Value)
            .Map(dest => dest.ShippingAddress, src => src.ShippingAddress)
            .Map(dest => dest.OrderItems, src => src.OrderItems)
            .Map(dest => dest.TotalAmount, src => src.TotalAmount)
            .Map(dest => dest.OrderStatusHistoryTrackings, src => src.OrderStatusHistoryTrackings);

        config.NewConfig<OrderItem, OrderItemResponse>()
            .Map(dest => dest.ProductId, src => src.ItemOrdered.ProductId.Value)
            .Map(dest => dest.ProductVariantId, src => src.ItemOrdered.ProductVariantId!.Value)
            .Map(dest => dest.ProductName, src => src.ItemOrdered.ProductName )
            .Map(dest => dest.ProductImage, src => src.ItemOrdered.ProductImage)
            .Map(dest => dest.ProductAttributes, src => src.ItemOrdered.ProductAttributes)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Map(dest => dest.SubTotal, src => src.SubTotal)
            .Map(dest => dest.TotalDiscount, src => src.TotalDiscount)
            .Map(dest => dest.Quantity, src => src.Quantity);

        config.NewConfig<OrderStatusHistoryTracking, OrderStatusHistoryTrackingResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Status, src => src.Status.Name)
            .Map(dest => dest.CreatedDateTime, src => src.CreatedDateTime);

        config.NewConfig<ShippingAddress, ShippingAddressResponse>();

        config.NewConfig<Guid, OrderId>()
            .Map(dest => dest, src => OrderId.Create(src));

        config.NewConfig<Guid, GetOrderByIdQuery>()
            .Map(dest => dest.OrderId, src => OrderId.Create(src));

        config.NewConfig<Guid, RefundOrderCommand>()
            .Map(dest => dest.OrderId, src => OrderId.Create(src));

        config.NewConfig<PagedList<Order>, PagedList<OrderResponse>>()
            .Map(dest => dest.Items, src => src.Items);

        config.NewConfig<(int, int), GetOrderListPagedQuery>()
            .Map(dest => dest.Page, src => src.Item1)
            .Map(dest => dest.PageSize, src => src.Item2);

        config.NewConfig<(Guid, GetMyOrdersRequest), GetOrdersByCustomerQuery>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src.Item1))
            .Map(dest => dest.OrderStatus, src => src.Item2.Status)
            .Map(dest => dest, src => src.Item2);

        config.NewConfig<(Guid, CreateOrderRequest), CreateOrderCommand>()
            .Map(dest => dest.CustomerId, src => CustomerId.Create(src.Item1))
            .Map(dest => dest.AddressId, src => src.Item2.AddressId);

        config.NewConfig<Guid, ConfirmPaymentInfoCommand>()
            .Map(dest => dest.OrderId, src => OrderId.Create(src));
    }
}
