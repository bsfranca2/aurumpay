using Ardalis.Result;

using AurumPay.Application.Customers;
using AurumPay.Application.Customers.UpdateAddress;
using AurumPay.Application.SeedWork;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Customers;
using AurumPay.Domain.Interfaces;

using MediatR;

namespace AurumPay.Application.CheckoutSessions.UpdateCustomerAddress;

internal sealed class UpdateCheckoutSessionCustomerAddressCommandHandler(
    ICheckoutContext checkoutContext,
    ISender sender
) : ICommandHandler<UpdateCheckoutSessionCustomerAddressCommand, Result<CustomerAddressDto>>
{
    public async Task<Result<CustomerAddressDto>> Handle(UpdateCheckoutSessionCustomerAddressCommand request,
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

        UpdateCustomerAddressCommand updateAddress = new(customerId.Value.Value, request.AddressId, request.Cep,
            request.AddressLine1, request.AddressLine2, request.Number, request.City, request.State, true);
        return await sender.Send(updateAddress, cancellationToken);
    }
}