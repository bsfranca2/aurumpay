using Ardalis.GuardClauses;

using AurumPay.Domain.Customers;
using AurumPay.Domain.SeedWork;
using AurumPay.Domain.Stores;

namespace AurumPay.Domain.CheckoutSessions;

public sealed class CheckoutSession : IEntity<CheckoutSessionId>
{
    private readonly HashSet<CartItem> _cartItems = [];

    public CheckoutSessionId Id { get; init; }
    public StoreId StoreId { get; init; }
    // TODO: Rename to DeviceFingerprint
    public string Fingerprint { get; init; }
    public CheckoutStatus Status { get; private set;  }
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.ToList();
    public CustomerId? CustomerId { get; private set; }

    private CheckoutSession(CheckoutSessionId id, StoreId storeId, string fingerprint, CheckoutStatus status,
        CustomerId? customerId)
    {
        Id = id;
        StoreId = storeId;
        Fingerprint = fingerprint;
        Status = status;
        CustomerId = customerId;
    }

    public static CheckoutSession Create(StoreId storeId, string fingerprint, ICollection<CartItem> cartItems)
    {
        Guard.Against.Zero(cartItems.Count, nameof(cartItems));

        CheckoutSession checkoutSession =
            new(new CheckoutSessionId(), storeId, fingerprint, CheckoutStatus.Pending, null);

        foreach (CartItem cartItem in cartItems)
        {
            checkoutSession._cartItems.Add(cartItem);
        }

        return checkoutSession;
    }

    public void IdentifyCustomer(Customer customer)
    {
        CustomerId = customer.Id;
    }
}

public readonly record struct CheckoutSessionId(Ulid Value)
{
    public CheckoutSessionId() : this(Ulid.NewUlid()) { }
}