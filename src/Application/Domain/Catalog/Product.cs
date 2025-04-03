using AurumPay.Application.Common;

namespace AurumPay.Application.Domain.Catalog;

public class Product : EntityBase
{
    public string Name { get; set; }
    
    public decimal Price { get; set; }

    private Product(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public static Product CreateExisting(Guid id, string name, decimal price) =>
        new(id, name, price);
    
    public static Product CreateNew(string name, decimal price) =>
        new (Guid.NewGuid(), name, price);
}