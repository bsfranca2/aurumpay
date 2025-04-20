using AurumPay.Domain.Customers;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Shared;
using AurumPay.Domain.Stores;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Infrastructure.EntityFramework.Repositories;

public class StoreCustomerService(DatabaseContext dbContext, IStoreContext storeContext) : IStoreCustomerService
{
    private StoreId StoreId => storeContext.GetCurrentStoreId(); 
    
    public async Task<Customer?> FindByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Customers
            .Where(c => c.StoreId == StoreId && c.Email == email)
            .FirstOrDefaultAsync(cancellationToken);
    }
}