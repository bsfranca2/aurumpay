using AurumPay.Domain.SeedWork;
using AurumPay.Domain.Shared;
using AurumPay.Domain.Stores;

namespace AurumPay.Domain.Customers;

public interface ICustomerRepository : IRepository<Customer, CustomerId>
{
    Task<Customer?> GetByIdWithAddressesAsync(CustomerId id);

    Task<Customer?> FindByEmailAsync(
        StoreId storeId,
        EmailAddress email,
        CancellationToken cancellationToken = default);
}