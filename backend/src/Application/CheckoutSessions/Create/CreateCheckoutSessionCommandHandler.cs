using Ardalis.Result;

using AurumPay.Application.SeedWork;
using AurumPay.Domain.Catalog;
using AurumPay.Domain.CheckoutSessions;
using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Stores;

namespace AurumPay.Application.CheckoutSessions.Create;

internal sealed class CreateCheckoutSessionCommandHandler(
    ICheckoutContext checkoutContext,
    ICheckoutSessionRepository checkoutSessionRepository,
    IStoreProductService storeProductService
) : ICommandHandler<CreateCheckoutSessionCommand, Result>
{
    public async Task<Result> Handle(CreateCheckoutSessionCommand request, CancellationToken ct)
    {
        StoreId currentStoreId = checkoutContext.Store.GetCurrentStoreId();

        bool isNewCartData = request.CartItems.Count > 0;

        Result<HashSet<CartItem>> cartItemsResult = isNewCartData
            ? await ValidateRequestCartItemsAsync(request, ct)
            : await checkoutContext.CartService.GetCartItemsAsync(ct);

        if (!cartItemsResult.IsSuccess)
        {
            return cartItemsResult.Map();
        }

        HashSet<CartItem>? cartItems = cartItemsResult.Value;

        if (cartItems.Count == 0)
        {
            return Result.Invalid(new ValidationError("Cart is empty or contains only invalid items for this store."));
        }
        
        // TODO: Validate if products are available and quantity.

        if (isNewCartData)
        {
            await checkoutContext.CartService.SetCartItemsAsync(cartItems);
        }

        CheckoutSession checkoutSession = CheckoutSession.Create(
            currentStoreId,
            checkoutContext.DeviceIdentity.GetFingerprint(),
            cartItems);

        await checkoutSessionRepository.CreateAsync(checkoutSession);
        await checkoutContext.SessionManager.EstablishSessionAsync(checkoutSession);

        return Result.Success();
    }

    private async Task<Result<HashSet<CartItem>>> ValidateRequestCartItemsAsync(
        CreateCheckoutSessionCommand request,
        CancellationToken ct)
    {
        HashSet<string> publicIds = request.CartItems.Select(ci => ci.Key).ToHashSet();
        Dictionary<string, ProductId>? productMappings = await storeProductService.MapPublicIdsToProductIdsAsync(publicIds, ct);

        if (productMappings == null || productMappings.Count == 0)
        {
            return Result.Invalid(new ValidationError("No valid products found for the provided items."));
        }

        return Result.Success(productMappings
            .Select(p => new CartItem(p.Value, request.CartItems[p.Key]))
            .ToHashSet());
    }
}