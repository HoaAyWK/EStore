using EStore.Application.Orders.Services;
using EStore.Contracts.Common;
using EStore.Contracts.Orders;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
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

    public async Task<List<OrderResponse>> GetOrdersByCriteriaAsync(
        CustomerId customerId,
        ProductId productId,
        ProductVariantId? productVariantId)
    {
        var orders = await _dbContext.Orders.AsNoTracking()
            .Where(order => order.CustomerId == customerId)
            .Where(order => order.OrderItems.Any(item =>
                item.ItemOrdered.ProductId == productId &&
                item.ItemOrdered.ProductVariantId! == productVariantId!))
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
                    .Select(tracking => new OrderStatusHistoryTrackingResponse(
                        tracking.Id.Value,
                        tracking.Status.Name,
                        tracking.CreatedDateTime))
                    .ToList()
            })
            .ToListAsync();

        return orders;
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
                .OrderByDescending(tracking => tracking.CreatedDateTime)
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
                    "orderNumber" => orderResponsesQuery.OrderByDescending(o => o.OrderNumber),
                    "customer" => orderResponsesQuery.OrderByDescending(o => o.Customer!.FirstName),
                    "createdDateTime" => orderResponsesQuery.OrderByDescending(o => o.CreatedDateTime),
                    "items" => orderResponsesQuery.OrderByDescending(o => o.OrderItems.Count),
                    _ => orderResponsesQuery.OrderByDescending(o => o.OrderNumber)
                };
            }
            else
            {
                orderResponsesQuery = orderBy switch
                {
                    "orderNumber" => orderResponsesQuery.OrderBy(o => o.OrderNumber),
                    "customer" => orderResponsesQuery.OrderBy(o => o.Customer!.FirstName),
                    "createdDateTime" => orderResponsesQuery.OrderBy(o => o.CreatedDateTime),
                    "items" => orderResponsesQuery.OrderBy(o => o.OrderItems.Count),
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
        string? order = null,
        string? orderBy = null,
        CancellationToken cancellationToken = default)
    {
        var ordersQuery = _dbContext.Orders
            .AsNoTracking()
            .Where(o => o.CustomerId == customerId);

        if (orderStatus is not null)
        {
            ordersQuery = ordersQuery.Where(o => o.OrderStatus == orderStatus);
        }

        if (!string.IsNullOrWhiteSpace(order) && order.ToLower() == "desc")
        {
            ordersQuery = orderBy switch
            {
                "createdDateTime" => ordersQuery.OrderByDescending(o => o.CreatedDateTime),
                "totalAmount" => ordersQuery.OrderByDescending(o => o.TotalAmount),
                _ => ordersQuery.OrderByDescending(o => o.OrderNumber)
            };
        }
        else
        {
            ordersQuery = orderBy switch
            {
                "createdDateTime" => ordersQuery.OrderBy(o => o.CreatedDateTime),
                "totalAmount" => ordersQuery.OrderBy(o => o.TotalAmount),
                _ => ordersQuery.OrderBy(o => o.OrderNumber)
            };
        }

        var totalItems = await ordersQuery.CountAsync(cancellationToken);
        
        var orders = await ordersQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return new PagedList<Order>(orders, page, pageSize, totalItems, totalPages);
    }

    public async Task<OrderStats> GetOrderStatsAsync(int lastDaysCount)
    {
        var totalOrders = await _dbContext.Orders.CountAsync();
        var orderGroups = await _dbContext.Orders
            .Where(order => order.CreatedDateTime.Date >= DateTime.UtcNow.Date.AddDays(-lastDaysCount))
            .GroupBy(order => order.CreatedDateTime.Date)
            .Select(group => new
            {
                Date = group.Key,
                Count = group.Count()
            })
            .ToListAsync();

        var startDate = DateTime.UtcNow.Date.AddDays(-lastDaysCount);
        var endDate = DateTime.UtcNow.Date;
        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(offset => startDate.AddDays(offset));

        var orderGroupsWithDefault = from date in dateRange
            join orderGroup in orderGroups
            on date equals orderGroup.Date into gj
            from subOrderGroup in gj.DefaultIfEmpty()
            select new
            {
                Date = date,
                Count = subOrderGroup != null ? subOrderGroup.Count : 0
            };

        var ordersPerDay = orderGroupsWithDefault
            .OrderBy(group => group.Date)
            .Select(group => group.Count)
            .ToList();

        var lastPreviousOrdersPerDay = await _dbContext.Orders
            .Where(order =>
                order.CreatedDateTime.Date >= DateTime.UtcNow.Date.AddDays(-lastDaysCount * 2) &&
                order.CreatedDateTime.Date < DateTime.UtcNow.Date.AddDays(-lastDaysCount))
            .GroupBy(order => order.CreatedDateTime.Date)
            .Select(group => new
            {
                Date = group.Key,
                Count = group.Count()
            })
            .OrderBy(group => group.Date)
            .Select(group => group.Count)
            .ToListAsync();

        var totalOrdersFromLastDays = ordersPerDay.Sum();
        var totalOrdersFromPreviousDays = lastPreviousOrdersPerDay.Sum();
        var compareToPreviousDays = 0.0;

        try
        {
            compareToPreviousDays = compareToPreviousDays = (totalOrdersFromLastDays - totalOrdersFromPreviousDays)
            / totalOrdersFromPreviousDays * 100;
        }
        catch (DivideByZeroException)
        {
            compareToPreviousDays = totalOrdersFromLastDays / 1.0 * 100;
        }
        
        var isIncreased = compareToPreviousDays > 0;

        return new OrderStats
        {
            TotalOrders = totalOrders,
            OrdersPerDay = ordersPerDay,
            FromDay = lastDaysCount,
            CompareToPreviousDays = compareToPreviousDays,
            IsIncreased = isIncreased
        };
    }

    public async Task<IncomeStats> GetIncomeStatsAsync(int lastDaysCount)
    {
        var orders = await _dbContext.Orders.AsNoTracking()
            .Where(order => order.OrderStatus == OrderStatus.Completed)
            .ToListAsync();

        var totalIncome = orders
            .Select(order =>
                order.OrderItems.Sum(
                    item => item.UnitPrice * item.Quantity - item.DiscountAmount * item.Quantity))
            .Sum();

        var incomeGroups = orders
            .Where(order => order.CreatedDateTime.Date >= DateTime.UtcNow.Date.AddDays(-lastDaysCount))
            .GroupBy(order => order.CreatedDateTime.Date)
            .Select(group => new
            {
                Date = group.Key,
                Income = group.Sum(order => order.TotalAmount - order.TotalDiscount)
            });

        var startDate = DateTime.UtcNow.Date.AddDays(-lastDaysCount);
        var endDate = DateTime.UtcNow.Date;
        var dateRange = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(offset => startDate.AddDays(offset));

        var incomeGroupsWithDefault = from date in dateRange
            join incomeGroup in incomeGroups
            on date equals incomeGroup.Date into gj
            from subIncomeGroup in gj.DefaultIfEmpty()
            select new
            {
                Date = date,
                Income = subIncomeGroup?.Income ?? 0
            };

        var incomePerDayDict = incomeGroupsWithDefault
            .ToDictionary(group => group.Date, group => group.Income);

        var incomePerDay = incomeGroupsWithDefault
            .OrderBy(group => group.Date)
            .Select(group => group.Income)
            .ToList();

        var lastPreviousIncomePerDay = orders
            .Where(order =>
                order.OrderStatus == OrderStatus.Completed &&
                order.CreatedDateTime.Date >= DateTime.UtcNow.Date.AddDays(-lastDaysCount * 2) &&
                order.CreatedDateTime.Date < DateTime.UtcNow.Date.AddDays(-lastDaysCount))
            .GroupBy(order => order.CreatedDateTime.Date)
            .Select(group => new
            {
                Date = group.Key,
                Income = group.Sum(order => order.TotalAmount - order.TotalDiscount)
            })
            .OrderBy(group => group.Date)
            .Select(group => group.Income)
            .ToList();

        var totalIncomeFromLastDays = incomePerDay.Sum();
        var totalIncomeFromPreviousDays = lastPreviousIncomePerDay.Sum();
        var compareToPreviousDays = 0m;

        try
        {
            compareToPreviousDays = compareToPreviousDays = (totalIncomeFromLastDays - totalIncomeFromPreviousDays)
            / totalIncomeFromPreviousDays * 100;
        }
        catch (DivideByZeroException)
        {
            compareToPreviousDays = (totalIncomeFromLastDays - totalIncomeFromPreviousDays) / 1m * 100;
        }
        
        var isIncreased = compareToPreviousDays > 0;

        return new IncomeStats
        {
            TotalIncome = totalIncome,
            IncomePerDay = incomePerDayDict,
            FromDay = lastDaysCount,
            CompareToPreviousDays = compareToPreviousDays,
            IsIncreased = isIncreased
        };
    }
}
