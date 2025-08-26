using Ardalis.Result;

using AurumPay.Application.Customers;
using AurumPay.Application.Data;
using AurumPay.Core;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Application.CheckoutSessions.Get;

internal sealed class GetCheckoutSessionQueryHandler(
    ICheckoutContext checkoutContext,
    IDatabaseContext dbContext
) : IQueryHandler<GetCheckoutSessionQuery, Result<CheckoutSessionDto>>
{
    public async Task<Result<CheckoutSessionDto>> Handle(GetCheckoutSessionQuery request,
        CancellationToken cancellationToken)
    {
        CheckoutSessionId? maybeSessionId = checkoutContext.SessionManager.GetCurrentSessionId();
        if (maybeSessionId == null)
        {
            return Result.Error("Session not found");
        }

        CheckoutSessionId sessionId = maybeSessionId.Value;

        CheckoutSessionDto? session = await dbContext
            .CheckoutSessions
            .Where(cs => cs.Id == sessionId)
            .Select(cs => new CheckoutSessionDto(
                cs.Status,
                cs.CartItems.Select(ci => new CartItemDto(
                    new CartItemProductDto(
                        ci.ProductId.Value,
                        dbContext.Products
                            .Where(p => p.Id == ci.ProductId)
                            .Select(p => p.Name)
                            .First(),
                        dbContext.Products
                            .Where(p => p.Id == ci.ProductId)
                            .Select(p => p.Price)
                            .First()
                    ),
                    ci.Quantity
                )),
                cs.CustomerId != null
                    ? dbContext.Customers
                        .Where(c => c.Id == cs.CustomerId)
                        .Select(c => new CustomerDto(
                            c.FullName,
                            c.Email.Value,
                            c.Cpf.Value,
                            c.PhoneNumber.Value,
                            c.Addresses.Select(a => new CustomerAddressDto(
                                a.Id.Value,
                                a.Cep.Value,
                                a.AddressLine1,
                                a.AddressLine2,
                                a.Number,
                                a.Neighborhood,
                                a.City,
                                a.State,
                                a.Recipient,
                                a.IsMain
                            )),
                            c.IsProspect
                        ))
                        .FirstOrDefault()
                    : null,
                cs.SelectedPaymentMethodId,
                cs.SelectedPaymentGatewayId
            ))
            .SingleOrDefaultAsync(cancellationToken);

        if (session == null)
        {
            await checkoutContext.SessionManager.EndSessionAsync();
            return Result.Error("Session not found");
        }

        return session;
    }
}