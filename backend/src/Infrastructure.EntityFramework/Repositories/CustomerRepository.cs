using AurumPay.Domain.Customers;
using AurumPay.Domain.Shared;
using AurumPay.Domain.Stores;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Infrastructure.EntityFramework.Repositories;

public class CustomerRepository(DatabaseContext context)
    : Repository<Customer, CustomerId, DatabaseContext>(context), ICustomerRepository
{
    public async Task<Customer?> GetByIdWithAddressesAsync(CustomerId id)
    {
        return await DbSet
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer?> FindByEmailAsync(
        StoreId storeId,
        EmailAddress email,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(c => c.StoreId == storeId && c.Email == email)
            .FirstOrDefaultAsync(cancellationToken);
    }
}