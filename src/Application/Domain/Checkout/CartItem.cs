using AurumPay.Application.Common;

namespace AurumPay.Application.Domain.Checkout;

public class CartItem : EntityBase
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public CartItem(Guid id, Guid productId, string productName, decimal unitPrice, int quantity)
    {
        Id = id;
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    } 
}