using AurumPay.Ordering.Domain.SeedWork;

namespace AurumPay.Ordering.Domain.Checkout;

public class CheckoutSession : EntityBase
{
    public Guid StoreId { get; private set; }
    public CheckoutStatus Status { get; private set; }
    private readonly List<CartItem> _cartItems;
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    private CheckoutSession(Guid id, Guid storeId, CheckoutStatus status)
    {
        Id = id;
        StoreId = storeId;
        Status = status;
        _cartItems = [];
    }

    public static CheckoutSession Create(Guid storeId)
    {
        return new CheckoutSession(Guid.NewGuid(), storeId, CheckoutStatus.Pending);
    }

    public void AddCartItem(Guid productId, int quantity)
    {
        _cartItems.Add(new CartItem(Guid.NewGuid(), Id, productId, quantity));
    }
}