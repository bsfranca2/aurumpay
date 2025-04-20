using AurumPay.Domain.Customers;

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
}