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
    IStoreProductValidator storeProductValidator
) : ICommandHandler<CreateCheckoutSessionCommand, Result>
{
    public async Task<Result> Handle(CreateCheckoutSessionCommand request,
        CancellationToken cancellationToken)
    {
        HashSet<CartItem> cartItems;

        if (request.CartItems.Count > 0)
        {
            cartItems = request.CartItems
                .Select(ci => new CartItem(new ProductId(ci.ProductId), ci.Quantity))
                .ToHashSet();

            // TODO: Change to a domain event handler
            await checkoutContext.CartService.SetCartItemsAsync(cartItems);
        }
        else
        {
            cartItems = await checkoutContext.CartService.GetCartItemsAsync(cancellationToken);
        }

        if (cartItems.Count == 0)
        {
            return Result.Invalid(new ValidationError("Cart is empty"));
        }

        StoreId currentStoreId = checkoutContext.Store.GetCurrentStoreId();

        IEnumerable<ProductId> productIds = cartItems.Select(ci => ci.ProductId);
        bool isValid = await storeProductValidator.ValidateProductsAsync(currentStoreId, productIds);
        if (!isValid)
        {
            return Result.Invalid(new ValidationError("One or more products are not valid"));
        }

        CheckoutSession checkoutSession =
            CheckoutSession.Create(currentStoreId, checkoutContext.DeviceIdentity.GetFingerprint(), cartItems);

        await checkoutSessionRepository.CreateAsync(checkoutSession);

        await checkoutContext.SessionManager.EstablishSessionAsync(checkoutSession);

        return Result.Success();
    }
}