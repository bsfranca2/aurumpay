using AurumPay.Domain.Customers;
using AurumPay.Domain.Shared;

namespace AurumPay.Domain.Interfaces;

public interface IStoreCustomerService
{
    Task<Customer?> FindByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default);
}