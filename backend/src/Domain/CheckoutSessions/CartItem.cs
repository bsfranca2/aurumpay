using Ardalis.GuardClauses;

using AurumPay.Core;
using AurumPay.Domain.Catalog;

namespace AurumPay.Domain.CheckoutSessions;

public sealed class CartItem : ValueObject
{
    public ProductId ProductId { get; }
    public int Quantity { get; }

    public CartItem(ProductId productId, int quantity)
    {
        ProductId = productId;
        Quantity = Guard.Against.NegativeOrZero(quantity);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProductId;
    }
}
