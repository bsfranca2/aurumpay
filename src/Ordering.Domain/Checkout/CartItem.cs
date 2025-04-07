using AurumPay.Ordering.Domain.SeedWork;

namespace AurumPay.Ordering.Domain.Checkout;

public class CartItem : EntityBase
{
    public Guid CheckoutSessionId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    public CartItem(Guid id, Guid checkoutSessionId, Guid productId, int quantity)
    {
        Id = id;
        CheckoutSessionId = checkoutSessionId;
        ProductId = productId;
        Quantity = quantity;
    } 
}