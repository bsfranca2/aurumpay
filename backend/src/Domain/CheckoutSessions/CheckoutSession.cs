using Ardalis.GuardClauses;

using AurumPay.Core;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Orders;
using AurumPay.Domain.Payments.Gateways;
using AurumPay.Domain.Payments.Methods;
using AurumPay.Domain.Stores;

namespace AurumPay.Domain.CheckoutSessions;

public sealed class CheckoutSession : IEntity<CheckoutSessionId>
{
    private readonly HashSet<CartItem> _cartItems = [];

    public CheckoutSessionId Id { get; }
    public StoreId StoreId { get; }
    public string DeviceFingerprint { get; private set; } // TODO: check set 
    public CheckoutStatus Status { get; private set; }
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.ToList();
    public CustomerId? CustomerId { get; private set; }

    public PaymentMethodId? SelectedPaymentMethodId { get; private set; }
    public PaymentGatewayId? SelectedPaymentGatewayId { get; private set; }

    public OrderId? OrderId { get; private set; }

    private CheckoutSession(CheckoutSessionId id, StoreId storeId, string deviceFingerprint, CheckoutStatus status,
        CustomerId? customerId = null, PaymentMethodId? selectedPaymentMethodId = null,
        PaymentGatewayId? selectedPaymentGatewayId = null, OrderId? orderId = null)
    {
        Id = id;
        StoreId = storeId;
        DeviceFingerprint = deviceFingerprint;
        Status = status;
        CustomerId = customerId;
        SelectedPaymentMethodId = selectedPaymentMethodId;
        SelectedPaymentGatewayId = selectedPaymentGatewayId;
        OrderId = orderId;
    }

    public static CheckoutSession Create(StoreId storeId, string fingerprint, ICollection<CartItem> cartItems)
    {
        Guard.Against.Zero(cartItems.Count, nameof(cartItems));

        CheckoutSession checkoutSession = new(new CheckoutSessionId(), storeId, fingerprint, CheckoutStatus.Pending);

        foreach (CartItem cartItem in cartItems)
        {
            checkoutSession._cartItems.Add(cartItem);
        }

        return checkoutSession;
    }

    public void IdentifyCustomer(Customer customer)
    {
        CustomerId = customer.Id;
        Status = CheckoutStatus.CustomerIdentified;
    }

    public void SelectPaymentMethod(PaymentMethodId paymentMethodId, PaymentGatewayId paymentGatewayId)
    {
        SelectedPaymentMethodId = paymentMethodId;
        SelectedPaymentGatewayId = paymentGatewayId;
        Status = CheckoutStatus.PaymentMethodSelected;
    }

    public void CreateOrder(OrderId orderId)
    {
        if (CustomerId == null)
        {
            throw new InvalidOperationException("Customer must be identified before creating order");
        }

        OrderId = orderId;
        Status = CheckoutStatus.OrderCreated;
    }

    public void MarkPaymentProcessing()
    {
        if (Status == CheckoutStatus.OrderCreated)
        {
            Status = CheckoutStatus.PaymentProcessing;
        }
    }

    public void MarkPaymentCompleted()
    {
        if (Status == CheckoutStatus.PaymentProcessing)
        {
            Status = CheckoutStatus.PaymentCompleted;
        }
    }

    public void MarkPaymentFailed()
    {
        if (Status == CheckoutStatus.PaymentProcessing)
        {
            Status = CheckoutStatus.PaymentFailed;
        }
    }

    public void Complete()
    {
        if (Status == CheckoutStatus.PaymentCompleted)
        {
            Status = CheckoutStatus.Completed;
        }
    }
}