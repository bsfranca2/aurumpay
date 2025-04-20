using Ardalis.Result;

using AurumPay.Application.Data;
using AurumPay.Application.SeedWork;
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
            return Result.Invalid();
        }

        CheckoutSessionId sessionId = maybeSessionId.Value;
        CheckoutSessionDto? session = await dbContext
            .CheckoutSessions
            .Include(cs => cs.CartItems)
            .Where(cs => cs.Id == sessionId)
            .Select(cs => new CheckoutSessionDto(
                cs.CartItems.Select(ci => new CartItemDto(
                    ci.ProductId.Value,
                    ci.Quantity
                ))
            ))
            .SingleOrDefaultAsync(cancellationToken);

        if (session == null)
        {
            await checkoutContext.SessionManager.EndSessionAsync();
            return Result.Invalid();
        }

        return session;
    }
}