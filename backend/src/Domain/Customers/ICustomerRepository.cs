using AurumPay.Domain.SeedWork;
using AurumPay.Domain.Stores;

namespace AurumPay.Domain.Customers;

public interface ICustomerRepository : IRepository<Customer, CustomerId> 
{
    Task<Customer?> GetByIdWithAddressesAsync(CustomerId id);
}