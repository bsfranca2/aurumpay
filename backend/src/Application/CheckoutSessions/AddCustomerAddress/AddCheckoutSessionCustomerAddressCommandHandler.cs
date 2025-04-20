using Ardalis.Result;

using AurumPay.Application.Customers;
using AurumPay.Application.Customers.AddAddress;
using AurumPay.Application.SeedWork;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Interfaces;

using MediatR;

namespace AurumPay.Application.CheckoutSessions.AddCustomerAddress;

internal sealed class AddCheckoutSessionCustomerAddressCommandHandler(
    ICheckoutContext checkoutContext,
    ISender sender
) : ICommandHandler<AddCheckoutSessionCustomerAddressCommand, Result<CustomerAddressDto>>
{
    public async Task<Result<CustomerAddressDto>> Handle(AddCheckoutSessionCustomerAddressCommand request,
        CancellationToken cancellationToken)
    {
        CheckoutSession? session = await checkoutContext.SessionManager.GetCurrentSessionAsync();
        if (session is null)
        {
            return Result.Invalid();
        }

        CustomerId? customerId = session.CustomerId;
        if (customerId is null)
        {
            return Result.Invalid();
        }

        AddCustomerAddressCommand addAddress = new(customerId.Value.Value, request.Cep, request.AddressLine1,
            request.AddressLine2, request.Number, request.City, request.State, true);
        return await sender.Send(addAddress, cancellationToken);
    }
}