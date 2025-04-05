using AurumPay.Application.Common;

namespace AurumPay.Application.Domain.Catalog;

public class Product : EntityBase
{
    public Guid StoreId { get; private set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    private Product(Guid id, Guid storeId, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
    
    public static Product CreateNew(Guid storeId, string name, decimal price) =>
        new (Guid.NewGuid(), storeId, name, price);
}