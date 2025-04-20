using AurumPay.Domain.Interfaces;
using AurumPay.Domain.Stores;

namespace AurumPay.Checkout.Api.Infrastructure.Services;

public class StoreContext(IHttpContextAccessor httpContextAccessor) : IStoreContext
{
    public StoreId GetCurrentStoreId()
    {
        var storeId = httpContextAccessor.HttpContext?.Items["StoreId"] as StoreId?;
        return storeId ?? throw new NullReferenceException(nameof(storeId));
    }
}