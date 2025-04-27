using Ardalis.Result;

using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Stores;

using NanoidDotNet;

using ZiggyCreatures.Caching.Fusion;

namespace AurumPay.Checkout.Api.Infrastructure.Services;

public class CartService(
    IFusionCache fusionCache,
    IHttpContextAccessor httpContextAccessor,
    IStoreContext storeContext
) : ICartService
{
    private const string CartIdHeaderKey = "X-Cart";
    private const int CartExpirationDays = 7;

    private string? GetCartId()
    {
        return httpContextAccessor.HttpContext?.Request.Headers[CartIdHeaderKey].FirstOrDefault();
    }

    public async Task<Result<HashSet<CartItem>>> GetCartItemsAsync(CancellationToken cancellationToken = default)
    {
        string? cartId = GetCartId();
        if (string.IsNullOrEmpty(cartId))
        {
            return Result.Invalid(new ValidationError("No active cart found."));
        }

        string cacheKey = GenerateCacheKey(storeContext.GetCurrentStoreId(), cartId);
        MaybeValue<HashSet<CartItem>> cartItems = await fusionCache.TryGetAsync<HashSet<CartItem>>(
            cacheKey,
            null,
            cancellationToken);

        HashSet<CartItem> lastCartItems = cartItems.GetValueOrDefault([]);

        if (lastCartItems.Count == 0)
        {
            return Result.Invalid(new ValidationError("No active cart found or previous cart is empty."));
        }

        return lastCartItems;
    }

    public async Task SetCartItemsAsync(HashSet<CartItem> cartItems)
    {
        string cartId = GetCartId() ?? Nanoid.Generate(size: 11);
        string cacheKey = GenerateCacheKey(storeContext.GetCurrentStoreId(), cartId);

        await fusionCache.SetAsync(
            cacheKey,
            cartItems,
            TimeSpan.FromDays(CartExpirationDays));

        httpContextAccessor.HttpContext?.Response.Headers.Append(CartIdHeaderKey, cartId);
    }

    private static string GenerateCacheKey(StoreId storeId, string cartId)
    {
        return $"Cart:{storeId.Value}:{cartId}";
    }
}