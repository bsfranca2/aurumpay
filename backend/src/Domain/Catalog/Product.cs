using AurumPay.Domain.SeedWork;
using AurumPay.Domain.Stores;

namespace AurumPay.Domain.Catalog;

public sealed class Product : IEntity<ProductId>
{
    public ProductId Id { get; set; }
    public StoreId StoreId { get; }
    public string Name { get; }
    public decimal Price { get; }

    public Product(ProductId id, StoreId storeId, string name, decimal price)
    {
        Id = id;
        StoreId = storeId;
        Name = name;
        Price = price;
    }
    
    // public static Product CreateNew(StoreId storeId, string name, decimal price) =>
    //     new (new ProductId(), storeId, name, price);
}

public readonly record struct ProductId(long Value);
