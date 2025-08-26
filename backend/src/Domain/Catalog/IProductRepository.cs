using AurumPay.Core;

namespace AurumPay.Domain.Catalog;

public interface IProductRepository : IRepository<Product, ProductId>
{
}