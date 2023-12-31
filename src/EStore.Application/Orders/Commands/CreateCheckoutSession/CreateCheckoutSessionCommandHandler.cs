using ErrorOr;
using EStore.Application.Carts.Services;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Application.Orders.Services;
using EStore.Domain.Common.Errors;
using EStore.Domain.OrderAggregate;
using EStore.Domain.OrderAggregate.Entities;
using EStore.Domain.OrderAggregate.Enumerations;
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
    private readonly IUnitOfWork _unitOfWork;

    public CreateCheckoutSessionCommandHandler(
        IPaymentService paymentService,
        ICartReadService cartReadService,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _paymentService = paymentService;
        _cartReadService = cartReadService;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
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

        if (cart.Items.Count == 0)
        {
            return Errors.Order.CartIsEmpty;
        }

        var validateCartItemResult = await _cartReadService.ValidatePurchasedItemsAsync(
            request.CustomerId,
            request.CartTotalAmount);

        if (validateCartItemResult.IsError)
        {
            return validateCartItemResult.Errors;
        }

        var shippingAddress = ShippingAddress.Create(
            street: "",
            city: "",
            state: "",
            country: "",
            zipCode: "");

        List<OrderItem> orderItems = new();

        foreach (var cartItem in cart.Items)
        {
            var itemOrdered = ItemOrdered.Create(
                productId: ProductId.Create(cartItem.ProductId),
                productVariantId: cartItem.ProductVariantId != null
                    ? ProductVariantId.Create(cartItem.ProductVariantId.Value)
                    : null,
                productName: cartItem.ProductName,
                productImage: "",
                productAttributes: cartItem.ProductAttributes);

            var orderItem = OrderItem.Create(itemOrdered, cartItem.Price, cartItem.Quantity);

            orderItems.Add(orderItem);
        }

        var order = Order.Create(
            customerId: request.CustomerId,
            status: OrderStatus.Pending,
            transactionId: null,
            shippingAddress: shippingAddress,
            orderItems: orderItems);
        

        await _orderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        string sessionUrl = await _paymentService.ProcessPaymentAsync(order);

        return sessionUrl;
    }
}
