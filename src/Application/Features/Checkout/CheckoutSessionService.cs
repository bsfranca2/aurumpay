using System.Text.Json;

using AurumPay.Application.Infrastructure.Cache;
using AurumPay.Application.Infrastructure.Persistence;
using AurumPay.Ordering.Domain.Checkout;

using Microsoft.EntityFrameworkCore;

namespace AurumPay.Application.Features.Checkout;

public class CheckoutSessionService(ICacheService cache, AppDbContext dbContext)
{
    public async Task<Guid> CreateNewSessionAsync(Guid storeId, List<CartItemDto> items)
    {
        CheckoutSession session = CheckoutSession.Create(storeId);
        foreach (CartItemDto cartItemDto in items)
        {
            session.AddCartItem(cartItemDto.ProductId, cartItemDto.Quantity);
        }

        dbContext.CheckoutSessions.Add(session);
        await dbContext.SaveChangesAsync();

        return session.Id;
    }

    public async Task<CheckoutSession?> GetSessionAsync(string sessionId)
    {
        string? data = await cache.GetAsync<string>($"checkout:{sessionId}");
        return data == null ? null : JsonSerializer.Deserialize<CheckoutSession>(data);
    }

    // public async Task<Guid?> RestoreSessionAsync(Guid storeId, string cartToken)
    // {
    //     string cacheKey = $"{storeId}_cart_{cartToken}";
    //     List<CartItem>? items = await cache.GetAsync<List<CartItem>>(cacheKey);
    //     if (items == null)
    //     {
    //         return null;
    //     }
    //
    //     CheckoutSession session = CheckoutSession.Create(storeId);
    //     dbContext.CheckoutSessions.Add(session);
    //     await dbContext.SaveChangesAsync();
    //
    //     return session.Id;
    // }

    public bool ValidateSessionAsync(string sessionId)
    {
        if (Guid.TryParse(sessionId, out Guid sessionGuid) && sessionGuid != Guid.Empty)
        {
            var result = dbContext.CheckoutSessions.Any(cs => cs.Id == sessionGuid);
            return result;
        }

        return false;
    }
}