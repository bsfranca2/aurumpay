using AurumPay.Core;
using AurumPay.Domain.Catalog;
using AurumPay.Domain.CheckoutSessions;

namespace AurumPay.Domain.Orders;

public class OrderItem : IEntity<OrderItemId>
{
    public OrderItemId Id { get; }
    public ProductId ProductId { get; }

    public string ProductName { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }
    public decimal TotalPrice => Quantity * UnitPrice;

    public DateTime CreatedOnUtc { get; }

    private OrderItem(OrderItemId id, ProductId productId, string productName, int quantity, decimal unitPrice,
        DateTime createdOnUtc)
    {
        Id = id;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        CreatedOnUtc = createdOnUtc;
    }

    public static OrderItem Create(CartItem cartItem, Product product)
    {
        return new OrderItem(new OrderItemId(), cartItem.ProductId, product.Name, cartItem.Quantity, product.Price,
            DateTime.UtcNow);
    }
}