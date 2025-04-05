using AurumPay.Application.Common;

namespace AurumPay.Application.Domain.Checkout;

public class CheckoutSession : EntityBase
{
    public Guid StoreId { get; private set; }
    public string SessionToken { get; private set; }
    public CheckoutStatus Status { get; private set; }
    private readonly List<CartItem> _cartItems;
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    public CheckoutSession(Guid id, Guid storeId, string sessionToken, CheckoutStatus status)
    {
        Id = id;
        StoreId = storeId;
        SessionToken = sessionToken;
        Status = status;
        _cartItems = [];
    }
}