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

namespace EStore.Application.Orders.Commands.CreateCheckoutSession;

public class CreateCheckoutSessionCommandHandler
    : IRequestHandler<CreateCheckoutSessionCommand, ErrorOr<string>>
{
    private readonly IPaymentService _paymentService;
    private readonly ICartReadService _cartReadService;
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrderSequenceService _orderSequenceService;
    private readonly IPublisher _publisher;

    public CreateCheckoutSessionCommandHandler(
        IPaymentService paymentService,
        ICartReadService cartReadService,
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IOrderSequenceService orderSequenceService,
        IPublisher publisher)
    {
        _paymentService = paymentService;
        _cartReadService = cartReadService;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _orderSequenceService = orderSequenceService;
        _publisher = publisher;
    }

    public async Task<ErrorOr<string>> Handle(
        CreateCheckoutSessionCommand request,
        CancellationToken cancellationToken)
    {
        var cart = await _cartReadService.GetByCustomerIdAsync(request.CustomerId);

        if (cart is null)
        {
            return Errors.Order.CartNotFound;
        }

        if (cart.Items.Count is 0)
        {
            return Errors.Order.CartIsEmpty;
        }

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

        var validateCartItemResult = await _cartReadService.ValidatePurchasedItemsAsync(
            request.CustomerId);

        if (validateCartItemResult.IsError)
        {
            return validateCartItemResult.Errors;
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
            paymentMethod: PaymentMethod.CreditCard);
        
        await _orderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _orderSequenceService.IncreaseLastOrderNumberAsync();
        await _publisher.Publish(
            new OrderCreatedDomainEvent(order.Id, customer.Id),
            cancellationToken);        

        string sessionUrl = await _paymentService.ProcessPaymentAsync(order);

        return sessionUrl;
    }
}
