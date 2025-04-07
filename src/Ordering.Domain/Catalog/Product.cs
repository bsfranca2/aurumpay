using AurumPay.Ordering.Domain.SeedWork;

namespace AurumPay.Ordering.Domain.Catalog;

public class Product : EntityBase
{
    public Guid StoreId { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    private Product(Guid id, Guid storeId, string name, decimal price)
    {
        Id = id;
        StoreId = storeId;
        Name = name;
        Price = price;
    }
    
    public static Product CreateNew(Guid storeId, string name, decimal price) =>
        new (Guid.NewGuid(), storeId, name, price);
}