using EStore.Application.Orders.Services;
using EStore.Contracts.Common;
using EStore.Contracts.Orders;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.ValueObjects;
using EStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Services;

internal sealed class OrderReadService : IOrderReadService
{
    private readonly EStoreDbContext _dbContext;

    public OrderReadService(EStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<PagedList<OrderResponse>> GetListPagedAsync(
        int page,
        int pageSize,
        string? orderStatus,
        string? order,
        string? orderBy,
        int? orderNumber)
    {
        var ordersQuery = _dbContext.Orders.AsNoTracking();

        if (orderNumber.HasValue)
        {
            ordersQuery = ordersQuery.Where(o =>
                o.OrderNumber.ToString().Contains(orderNumber.Value.ToString()));
        }

        if (!string.IsNullOrEmpty(orderStatus))
        {
            var status = OrderStatus.FromName(orderStatus);

            if (status is not null)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderStatus == status);
            }
        }

        var orderResponsesQuery = ordersQuery.Select(order => new OrderResponse
        {
            Id = order.Id.Value,
            OrderNumber = order.OrderNumber,
            CreatedDateTime = order.CreatedDateTime,
            UpdatedDateTime = order.UpdatedDateTime,
            OrderStatus = order.OrderStatus.Name,
            TotalAmount = order.TotalAmount,
            CustomerId = order.CustomerId.Value,
            Customer = _dbContext.Customers.AsNoTracking()
                .Where(customer => customer.Id == order.CustomerId)
                .Select(customer => new CustomerResponse(
                    customer.Id.Value,
                    customer.FirstName,
                    customer.LastName,
                    customer.Email,
                    customer.AvatarUrl))
                .SingleOrDefault(),
            OrderItems = order.OrderItems
                .Select(orderItem => new OrderItemResponse(
                    orderItem.ItemOrdered.ProductId.Value,
                    orderItem.ItemOrdered.ProductVariantId! == null!
                        ? null
                        : orderItem.ItemOrdered.ProductVariantId.Value,
                    orderItem.ItemOrdered.ProductName,
                    orderItem.ItemOrdered.ProductImage!,
                    orderItem.ItemOrdered.ProductAttributes,
                    orderItem.UnitPrice,
                    orderItem.SubTotal,
                    orderItem.TotalDiscount,
                    orderItem.Quantity))
                .ToList(),
            ShippingAddress = new ShippingAddressResponse(
                order.ShippingAddress.ReceiverName,
                order.ShippingAddress.PhoneNumber,
                order.ShippingAddress.Street,
                order.ShippingAddress.City,
                order.ShippingAddress.State,
                order.ShippingAddress.Country,
                order.ShippingAddress.ZipCode),
            PaymentMethod = order.PaymentMethod.Name,
            PaymentStatus = order.PaymentStatus.Name,
            OrderStatusHistoryTrackings = order.OrderStatusHistoryTrackings
                .Select(tracking => new OrderStatusHistoryTrackingResponse(
                    tracking.Id.Value,
                    tracking.Status.Name,
                    tracking.CreatedDateTime))
                .ToList()
        });

        if (!string.IsNullOrEmpty(order))
        {
            if (!string.IsNullOrWhiteSpace(order) && order.ToLower() == "desc")
            {
                orderResponsesQuery = orderBy switch
                {
                    "customer" => orderResponsesQuery.OrderByDescending(o => o.Customer!.FirstName),
                    "createdDateTime" => orderResponsesQuery.OrderByDescending(o => o.CreatedDateTime),
                    "items" => orderResponsesQuery.OrderByDescending(o => o.OrderItems.Count),
                    "totalAmount" => orderResponsesQuery.OrderByDescending(o => o.TotalAmount),
                    _ => orderResponsesQuery.OrderByDescending(o => o.OrderNumber)
                };
            }
            else
            {
                orderResponsesQuery = orderBy switch
                {
                    "customer" => orderResponsesQuery.OrderBy(o => o.Customer!.FirstName),
                    "createdDateTime" => orderResponsesQuery.OrderBy(o => o.CreatedDateTime),
                    "items" => orderResponsesQuery.OrderBy(o => o.OrderItems.Count),
                    "totalAmount" => orderResponsesQuery.OrderBy(o => o.TotalAmount),
                    _ => orderResponsesQuery.OrderBy(o => o.OrderNumber)
                };
            }
        }
        
        var orderResponses = await orderResponsesQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalItems =  await ordersQuery.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return new PagedList<OrderResponse>(orderResponses, page, pageSize, totalItems, totalPages);
    }

    public async Task<OrderResponse?> GetByIdAsync(OrderId orderId)
    {
        return await _dbContext.Orders.AsNoTracking()
            .Where(order => order.Id == orderId)
            .Select(order => new OrderResponse
            {
                Id = order.Id.Value,
                OrderNumber = order.OrderNumber,
                CreatedDateTime = order.CreatedDateTime,
                UpdatedDateTime = order.UpdatedDateTime,
                OrderStatus = order.OrderStatus.Name,
                TotalAmount = order.TotalAmount,
                CustomerId = order.CustomerId.Value,
                Customer = _dbContext.Customers.AsNoTracking()
                    .Where(customer => customer.Id == order.CustomerId)
                    .Select(customer => new CustomerResponse(
                        customer.Id.Value,
                        customer.FirstName,
                        customer.LastName,
                        customer.Email,
                        customer.AvatarUrl))
                    .SingleOrDefault(),
                OrderItems = order.OrderItems
                    .Select(orderItem => new OrderItemResponse(
                        orderItem.ItemOrdered.ProductId.Value,
                        orderItem.ItemOrdered.ProductVariantId! == null!
                            ? null
                            : orderItem.ItemOrdered.ProductVariantId.Value,
                        orderItem.ItemOrdered.ProductName,
                        orderItem.ItemOrdered.ProductImage!,
                        orderItem.ItemOrdered.ProductAttributes,
                        orderItem.UnitPrice,
                        orderItem.SubTotal,
                        orderItem.TotalDiscount,
                        orderItem.Quantity))
                    .ToList(),
                ShippingAddress = new ShippingAddressResponse(
                    order.ShippingAddress.ReceiverName,
                    order.ShippingAddress.PhoneNumber,
                    order.ShippingAddress.Street,
                    order.ShippingAddress.City,
                    order.ShippingAddress.State,
                    order.ShippingAddress.Country,
                    order.ShippingAddress.ZipCode),
                PaymentMethod = order.PaymentMethod.Name,
                PaymentStatus = order.PaymentStatus.Name,
                OrderStatusHistoryTrackings = order.OrderStatusHistoryTrackings
                    .OrderByDescending(tracking => tracking.CreatedDateTime)
                    .Select(tracking => new OrderStatusHistoryTrackingResponse(
                        tracking.Id.Value,
                        tracking.Status.Name,
                        tracking.CreatedDateTime))
                    .ToList()    
            })
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<Order>> GetByCustomerIdAsync(
        CustomerId customerId,
        int page,
        int pageSize,
        OrderStatus? orderStatus = null,
        CancellationToken cancellationToken = default)
    {
        var ordersQuery = _dbContext.Orders
            .AsNoTracking()
            .Where(o => o.CustomerId == customerId);

        if (orderStatus is not null)
        {
            ordersQuery = ordersQuery.Where(o => o.OrderStatus == orderStatus);
        }

        var totalItems = await ordersQuery.CountAsync(cancellationToken);
        
        var orders = await ordersQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return new PagedList<Order>(orders, page, pageSize, totalItems, totalPages);
    }
}
