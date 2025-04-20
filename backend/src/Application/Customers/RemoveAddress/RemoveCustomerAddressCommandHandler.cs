using Ardalis.Result;

using AurumPay.Application.SeedWork;
using AurumPay.Domain.Customers;

namespace AurumPay.Application.Customers.RemoveAddress;

internal sealed class RemoveCustomerAddressCommandHandler(ICustomerRepository customerRepository)
    : ICommandHandler<RemoveCustomerAddressCommand, Result>
{
    public async Task<Result> Handle(RemoveCustomerAddressCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await customerRepository.GetByIdWithAddressesAsync(new CustomerId(request.CustomerId));

        if (customer is null)
        {
            return Result.Invalid(new ValidationError("Customer not found"));
        }

        customer.RemoveAddress(new CustomerAddressId(request.AddressId));

        await customerRepository.UpdateAsync(customer);

        return Result.Success();
    }
}