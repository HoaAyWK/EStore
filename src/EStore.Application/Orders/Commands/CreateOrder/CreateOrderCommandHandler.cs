using ErrorOr;
using EStore.Application.Carts.Services;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Application.Common.Interfaces.Services;
using EStore.Application.Orders.Services;
using EStore.Domain.Common.Errors;
using EStore.Domain.CustomerAggregate.Repositories;
using EStore.Domain.CustomerAggregate.ValueObjects;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Entities;
using EStore.Domain.OrderAggregate.Enumerations;
using EStore.Domain.OrderAggregate.Events;
using EStore.Domain.OrderAggregate.Repositories;
using EStore.Domain.OrderAggregate.ValueObjects;
using EStore.Domain.ProductAggregate.ValueObjects;
using MediatR;

namespace EStore.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, ErrorOr<Order>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICartReadService _cartReadService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IPublisher _publisher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderSequenceService _orderSequenceService;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        ICartReadService cartReadService,
        IDateTimeProvider dateTimeProvider,
        IPublisher publisher,
        IUnitOfWork unitOfWork,
        IOrderSequenceService orderSequenceService)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _cartReadService = cartReadService;
        _dateTimeProvider = dateTimeProvider;
        _publisher = publisher;
        _unitOfWork = unitOfWork;
        _orderSequenceService = orderSequenceService;
    }

    public async Task<ErrorOr<Order>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

        if (customer is null)
        {
            return Errors.Customer.NotFound;
        }
        
        var addressId = AddressId.Create(request.AddressId);
        var address = customer.Addresses
            .Where(address => address.Id == addressId)
            .SingleOrDefault();

        if (address is null)
        {
            return Errors.Customer.AddressNotFound;
        }

        var cart = await _cartReadService.GetByCustomerIdAsync(customer.Id);

        if (cart is null)
        {
            return Errors.Cart.NotFound;
        }

        if (cart.Items.Count is 0)
        {
            return Errors.Order.CartIsEmpty;
        }

        var shippingAddress = ShippingAddress.Create(
            receiverName: address.ReceiverName,
            phoneNumber: address.PhoneNumber,
            street: address.Street,
            city: address.City,
            state: address.State,
            country: address.Country,
            zipCode: address.ZipCode);

        List<OrderItem> orderItems = new();

        foreach (var cartItem in cart.Items)
        {
            var itemOrdered = ItemOrdered.Create(
                productId: ProductId.Create(cartItem.ProductId),
                productVariantId: cartItem.ProductVariantId != null
                    ? ProductVariantId.Create(cartItem.ProductVariantId.Value)
                    : null,
                productName: cartItem.ProductName,
                productImage: cartItem.ProductImageUrl!,
                productAttributes: cartItem.ProductAttributes);

            var discountAmount = cartItem.FinalPrice - cartItem.BasePrice;

            var orderItem = OrderItem.Create(
                itemOrdered,
                cartItem.FinalPrice,
                discountAmount,
                cartItem.Quantity);

            orderItems.Add(orderItem);
        }

        var orderNumber = await _orderSequenceService.GetNextOrderNumberAsync();

        var order = Order.Create(
            orderNumber: orderNumber,
            customerId: request.CustomerId,
            status: OrderStatus.Pending,
            transactionId: null,
            shippingAddress: shippingAddress,
            orderItems: orderItems,
            orderedDateTime: _dateTimeProvider.UtcNow,
            paymentMethod: PaymentMethod.CashOnDelivery);

        await _orderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // Need to publish the event immediately after the order is created
        await _publisher.Publish(
            new OrderCreatedDomainEvent(order.Id, customer.Id),
            cancellationToken);

        await _orderSequenceService.IncreaseLastOrderNumberAsync();

        return order;
    }
}
