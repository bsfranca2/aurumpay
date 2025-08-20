using AurumPay.Domain.Catalog;

namespace AurumPay.Infrastructure.EntityFramework.Repositories;

public class ProductRepository(DatabaseContext context)
    : Repository<Product, ProductId, DatabaseContext>(context), IProductRepository
{
}